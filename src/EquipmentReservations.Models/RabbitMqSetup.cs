namespace EquipmentReservations.Models
{
  /// <summary>
  /// Klasa konfiguracyjna dla połączeń z RabbitMQ.
  /// </summary>
  public class RabbitMqSetup
  {
    /// <summary>
    /// Ciąg połączenia do serwera RabbitMQ typu: amqp://username:password@hostname:port/vhost
    /// </summary>
    public string ConnectionString { get; set; } = null!;

    /// <summary>
    /// Nazwa exchange'a.
    /// </summary>
    public string ExchangeName { get; set; } = null!;

    /// <summary>
    /// Nazwa exchange'a dla wymiany DLQ.
    /// </summary>
    public string DlqExchangeName { get; set; } = null!;

    /// <summary>
    /// Nazwa kolejki.
    /// </summary>
    public string QueueName { get; set; } = null!;

    /// <summary>
    /// Nazwa kolejki DLQ.
    /// </summary>
    public string DlqQueueName { get; set; } = null!;

    /// <summary>
    /// RoutingKey.
    /// </summary>
    public string RoutingKey { get; set; } = null!;

    /// <summary>
    /// Czas między próbami automatycznego odzyskiwania połączenia.
    /// </summary>
    public int NetworkRecoveryInterval { get; set; }
  }
}
