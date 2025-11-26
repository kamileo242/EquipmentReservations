using System.Runtime.Serialization;

namespace EquipmentReservations.Models.Exceptions
{
  /// <summary>
  /// Klasa wyjątku odpowiedzialnego za błędy z połaczeniem Rabbit.
  /// </summary>
  public class RabbitException : BaseException
  {
    public RabbitException(string message = null) : base(message) { }

    public RabbitException(Exception innerException, string message = null) : base(innerException, message) { }

    public RabbitException(SerializationInfo info, StreamingContext context) : base(info, context) { }
  }
}
