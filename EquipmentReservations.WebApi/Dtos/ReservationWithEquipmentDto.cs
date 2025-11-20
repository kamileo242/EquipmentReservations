namespace EquipmentReservations.WebApi.Dtos
{
    /// <summary>
    /// Reprezentuje rezerwację wraz z pełnymi informacjami o sprzęcie.
    /// </summary>
    public record ReservationWithEquipmentDto
    {
        /// <summary>
        /// Identyfikator rezerwacji.
        /// </summary>
        public string Id { get; init; } = string.Empty;

        /// <summary>
        /// Sprzęt, którego dotyczy rezerwacja.
        /// </summary>
        public EquipmentDto Equipment { get; init; }

        /// <summary>
        /// Identyfikator użytkownika rezerwującego.
        /// </summary>
        public string UserId { get; init; } = string.Empty;

        /// <summary>
        /// Czas rozpoczęcia rezerwacji.
        /// </summary>
        public DateTime Start { get; init; }

        /// <summary>
        /// Czas zakończenia rezerwacji.
        /// </summary>
        public DateTime? End { get; init; }

        /// <summary>
        /// Status rezerwacji.
        /// </summary>
        public string ReservationStatus { get; init; } = string.Empty;

        /// <summary>
        /// Data utworzenia rezerwacji.
        /// </summary>
        public DateTime CreatedAt { get; init; }

        /// <summary>
        /// Data ostatniej modyfikacji rezerwacji.
        /// </summary>
        public DateTime UpdatedAt { get; init; }
    }
}
