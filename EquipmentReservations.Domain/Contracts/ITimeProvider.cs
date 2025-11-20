namespace EquipmentReservations.Domain
{
    /// <summary>
    /// Interfejs zwracający aktualny czas.
    /// </summary>
    public interface ITimeProvider
    {
        /// <summary>
        /// Zwraca aktualny czas
        /// </summary>
        /// <returns></returns>
        DateTime GetDateTime();
    }
}
