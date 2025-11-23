namespace EquipmentReservations.WebApi.Dtos
{
    /// <summary>
    /// Reprezentuje dane potrzebne do utworzenia lub modyfikacji rezerwacji sprzętu.
    /// </summary>
    public record ReservationStoreDto
    {
        /// <summary>
        /// Identyfikator sprzętu, który ma zostać zarezerwowany.
        /// </summary>
        public string? EquipmentId { get; init; }

        /// <summary>
        /// Identyfikator użytkownika dokonującego rezerwacji.
        /// </summary>
        public string? UserId { get; init; }

        /// <summary>
        /// Data i godzina rozpoczęcia rezerwacji.
        /// </summary>
        public DateTime? Start { get; init; }

        /// <summary>
        /// Data i godzina zakończenia rezerwacji.
        /// </summary>
        public DateTime? End { get; init; }
    }
}
