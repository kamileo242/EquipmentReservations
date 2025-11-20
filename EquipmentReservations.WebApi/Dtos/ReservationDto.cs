namespace EquipmentReservations.WebApi.Dtos
{
    /// <summary>
    /// Reprezentuje rezerwację sprzętu.
    /// </summary>
    public record ReservationDto
    {
        /// <summary>
        /// Identyfikator rezerwacji.
        /// </summary>
        public string Id { get; init; } = string.Empty;

        /// <summary>
        /// Nazwa zarezerwowanego sprzętu.
        /// </summary>
        public string? EquipmentId { get; init; }

        /// <summary>
        /// Nazwa zarezerwowanego sprzętu.
        /// </summary>
        public string? EquipmentName { get; init; }

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
