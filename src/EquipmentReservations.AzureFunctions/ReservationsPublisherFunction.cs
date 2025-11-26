using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Net;
using System.Text.Json;

namespace EquipmentReservations.AzureFunctions;

public class ReservationsPublisherFunction
{
  private readonly ServiceBusClient serviceBusClient;
  private const string TopicName = "reservations-topic";

  public ReservationsPublisherFunction(ServiceBusClient serviceBusClient)
  {
    this.serviceBusClient = serviceBusClient;
  }

  [Function("PublishReservation")]
  public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req)
  {
    var response = req.CreateResponse();

    try
    {
      string body;
      using (var reader = new StreamReader(req.Body))
      {
        body = await reader.ReadToEndAsync();
      }

      var evt = JsonSerializer.Deserialize<ReservationEvent>(body, new JsonSerializerOptions
      {
        PropertyNameCaseInsensitive = true
      });

      if (evt == null)
      {
        response.StatusCode = HttpStatusCode.BadRequest;
        await response.WriteStringAsync("Niepoprawny payload");
        return response;
      }

      var sender = serviceBusClient.CreateSender(TopicName);
      var messageBody = JsonSerializer.Serialize(evt);
      var message = new ServiceBusMessage(messageBody)
      {
        ContentType = "application/json"
      };
      message.ApplicationProperties["action"] = evt.Action;

      await sender.SendMessageAsync(message);

      response.StatusCode = HttpStatusCode.Accepted;
      await response.WriteStringAsync("Wys³ano rezerwacjê do Service Bus");
      return response;
    }
    catch
    {
      response.StatusCode = HttpStatusCode.InternalServerError;
      await response.WriteStringAsync("B³¹d serwera");
      return response;
    }
  }
}
