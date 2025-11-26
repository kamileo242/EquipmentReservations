using EquipmentReservations.Models;
using EquipmentReservations.Models.Exceptions;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace EquipmentReservations.RabbitMq.Services
{
  public class ReservationRabbitPublisher : IReservationRabbitPublisher
  {
    private readonly RabbitMqSetup rabbitMqSetup;
    private readonly ConnectionFactory connectionFactory;

    public ReservationRabbitPublisher(RabbitMqSetup rabbitMqSetup)
    {
      this.rabbitMqSetup = rabbitMqSetup;

      connectionFactory = new ConnectionFactory()
      {
        Uri = new Uri(rabbitMqSetup.ConnectionString),
        AutomaticRecoveryEnabled = true,
        NetworkRecoveryInterval = TimeSpan.FromSeconds(rabbitMqSetup.NetworkRecoveryInterval)
      };
    }
    public async Task PublishReservationAsync(Reservation reservation)
    {
      try
      {
        using var connection = connectionFactory.CreateConnection();
        using var channel = connection.CreateModel();

        ConfigureRabbitMq(channel);
        var messageBody = JsonSerializer.Serialize(reservation);
        var body = Encoding.UTF8.GetBytes(messageBody);

        var props = channel.CreateBasicProperties();
        props.Persistent = true;

        channel.BasicPublish(
            exchange: rabbitMqSetup.ExchangeName,
            routingKey: rabbitMqSetup.RoutingKey,
            basicProperties: props,
            body: body);
      }
      catch (Exception ex)
      {
        throw new RabbitException($"Błąd połączenia z RabbitMq. {ex.Message}");
      }
    }

    private void ConfigureRabbitMq(IModel channel)
    {
      channel.ExchangeDeclare(
          exchange: rabbitMqSetup.ExchangeName,
          type: ExchangeType.Direct,
          durable: true,
          autoDelete: false);
    }
  }
}
