using System.Runtime.Serialization;

namespace EquipmentReservations.Models.Exceptions
{
    /// <summary>
    /// Klasa wyjątku odpowiedzialnego za współbieżność.
    /// </summary>
    public class ConcurrencyConflictException : BaseException
    {
        public ConcurrencyConflictException(string message = null) : base(message) { }

        public ConcurrencyConflictException(Exception innerException, string message = null) : base(innerException, message) { }

        public ConcurrencyConflictException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
