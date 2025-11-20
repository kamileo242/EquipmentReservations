namespace EquipmentReservations.WebApi.Dtos
{
    /// <summary>
    /// Reprezentuje sprzęt dostępny do rezerwacji.
    /// </summary>
    public record EquipmentDto
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
        /// Identyfikator kategorii, do której należy sprzęt.
        /// </summary>
        public string CategoryId { get; init; } = string.Empty;

        /// <summary>
        /// Nazwa kategorii, do której należy sprzęt.
        /// </summary>
        public string? CategoryName { get; init; }
    }
}
