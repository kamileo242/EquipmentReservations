namespace EquipmentReservations.Domain.Services
{
    public class TimeProvider : ITimeProvider
    {
        public DateTime GetDateTime()
        {
            return DateTime.Now;
        }
    }
}
