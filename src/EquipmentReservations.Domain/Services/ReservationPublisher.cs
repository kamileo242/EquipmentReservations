using EquipmentReservations.Models;
using System.Text;
using System.Text.Json;

namespace EquipmentReservations.Domain.Services
{
  public class ReservationPublisher : IReservationPublisher
  {
    private readonly HttpClient httpClient;
    private readonly string publisherUrl;
    private readonly ITimeProvider timeProvider;

    public ReservationPublisher(HttpClient httpClient, string publisherUrl, ITimeProvider timeProvider)
    {
      this.httpClient = httpClient;
      this.publisherUrl = publisherUrl;
      this.timeProvider = timeProvider;
    }

    public async Task PublishReservationAsync(Reservation reservation, string action)
    {
      var evt = new ReservationEvent
      {
        Id = reservation.Id,
        EquipmentId = reservation.EquipmentId,
        UserId = reservation.UserId,
        Start = reservation.Start,
        End = reservation.End,
        ReservationStatus = reservation.ReservationStatus.ToString(),
        Action = action,
        OccurredAt = timeProvider.GetDateTime()
      };

      var json = JsonSerializer.Serialize(evt);
      var content = new StringContent(json, Encoding.UTF8, "application/json");

      var response = await httpClient.PostAsync(publisherUrl, content);
      response.EnsureSuccessStatusCode();
    }
  }
}
