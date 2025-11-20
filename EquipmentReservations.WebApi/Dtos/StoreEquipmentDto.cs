namespace EquipmentReservations.WebApi.Dtos
{
    /// <summary>
    /// Reprezentuje dodawany sprzęt dostępny do rezerwacji.
    /// </summary>
    public record StoreEquipmentDto
    {
        /// <summary>
        /// Nazwa obiektu.
        /// </summary>
        public string? Name { get; init; }

        /// <summary>
        /// Opis obiektu.
        /// </summary>
        public string? Description { get; init; }

        /// <summary>
        /// Identyfikator kategorii do której należy sprzęt.
        /// </summary>
        public string? CategoryId { get; init; }
    }
}
