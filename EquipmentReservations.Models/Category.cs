namespace EquipmentReservations.Models
{
    /// <summary>
    /// Kategoria obiektu.
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Identyfikator.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Nazwa kategorii.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Identyfikator kategorii nadrzędnej.
        /// </summary>
        public Guid? ParentCategoryId { get; set; }


        /// <summary>
        /// Kategoria nadrzędna.
        /// </summary>
        public Category? ParentCategory { get; set; }

        /// <summary>
        /// Kategorie podrzędne.
        /// </summary>
        public ICollection<Category>? SubCategories { get; set; }

        /// <summary>
        /// Lista sprzętów w tej kategorii. 
        /// </summary>
        public ICollection<Equipment>? Equipments { get; set; }
    }
}
