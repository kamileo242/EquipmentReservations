namespace EquipmentReservations.WebApi.Dtos
{
    /// <summary>
    /// Reprezentuje sprzęt z kategorią oraz rezerwacjami
    /// </summary>
    public record FullEquipmentDto
    {
        /// <summary>
        /// Identyfikator sprzętu.
        /// </summary>
        public string Id { get; init; } = string.Empty;

        /// <summary>
        /// Nazwa sprzętu.
        /// </summary>
        public string Name { get; init; } = string.Empty;

        /// <summary>
        /// Opis sprzętu.
        /// </summary>
        public string? Description { get; init; }

        /// <summary>
        /// Kategoria, do której należy sprzęt.
        /// </summary>
        public CategoryDto Category { get; init; }

        /// <summary>
        /// Lista rezerwacji na dany sprzęt.
        /// </summary>
        public List<ReservationDto> Reservations { get; init; }
    }
}
