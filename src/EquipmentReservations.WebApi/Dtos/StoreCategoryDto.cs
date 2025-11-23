namespace EquipmentReservations.WebApi.Dtos
{
    /// <summary>
    /// Reprezentuje dodawaną lub aktualizowaną kategorię sprzętu lub obiektu.
    /// </summary>
    public record StoreCategoryDto
    {
        /// <summary>
        /// Nazwa kategorii.
        /// </summary>
        public string? Name { get; init; }

        /// <summary>
        /// Identyfikator kategorii nadrzędnej.
        /// </summary>
        public string? ParentCategoryId { get; init; }
    }
}
