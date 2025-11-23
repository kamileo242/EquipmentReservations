namespace EquipmentReservations.Models
{
    /// <summary>
    /// Status rezerwacji.
    /// </summary>
    public enum ReservationStatus
    {
        /// <summary>
        /// Oczekująca na zatwierdzenie.
        /// </summary>
        Pending = 0,

        /// <summary>
        /// Zatwierdzona.
        /// </summary>
        Approved = 1,

        /// <summary>
        /// Anulowana.
        /// </summary>
        Cancelled = 2,

        /// <summary>
        /// Sprzęt został zwrócony
        /// </summary>
        Returned = 3,
    }
}
