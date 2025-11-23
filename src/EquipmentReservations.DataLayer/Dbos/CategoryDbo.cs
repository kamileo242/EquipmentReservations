using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EquipmentReservations.DataLayer.Dbos
{
    /// <summary>
    /// Reprezentacja kategorii w bazie danych.
    /// </summary>
    public class CategoryDbo
    {
        /// <summary>
        /// Identyfikator.
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Nazwa kategorii.
        /// </summary>
        [Required]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Identyfikator kategorii nadrzędnej.
        /// </summary>
        public Guid? ParentCategoryId { get; set; }

        /// <summary>
        /// Reprezentacja kategorii nadrzędnej w bazie danych.
        /// </summary>
        [ForeignKey(nameof(ParentCategoryId))]
        public virtual CategoryDbo? ParentCategory { get; set; }

        /// <summary>
        /// Reprezentacje kategorii podrzędnych w bazie danych.
        /// </summary>
        public virtual ICollection<CategoryDbo>? SubCategories { get; set; }

        /// <summary>
        /// Reprezentacje sprzętów w tej kategorii w bazie danych. 
        /// </summary>
        public virtual ICollection<EquipmentDbo>? Equipments { get; set; }

        /// <summary>
        /// Wersja rekordu dla kontroli współbieżności.
        /// </summary>
        [Timestamp]
        public byte[] RowVersion { get; set; } = null!;
    }
}
