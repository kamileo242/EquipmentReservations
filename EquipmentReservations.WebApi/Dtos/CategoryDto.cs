namespace EquipmentReservations.WebApi.Dtos
{
    /// <summary>
    /// Reprezentuje kategorię sprzętu lub obiektu.
    /// </summary>
    public record CategoryDto
    {
        /// <summary>
        /// Identyfikator kategorii.
        /// </summary>
        public string Id { get; init; } = string.Empty;

        /// <summary>
        /// Nazwa kategorii.
        /// </summary>
        public string Name { get; init; } = string.Empty;
    }
}