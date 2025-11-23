using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EquipmentReservations.DataLayer.Dbos
{
    /// <summary>
    /// Reprezentacja obiektu w bazie danych.
    /// </summary>
    public class EquipmentDbo
    {
        /// <summary>
        /// Identyfikator obiektu.
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Nazwa obiektu.
        /// </summary>
        [Required]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Opis obiektu.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Opis obiektu.
        /// </summary>
        [Required]
        public Guid CategoryId { get; set; }

        /// <summary>
        /// Kategoria do której należy obiekt.
        /// </summary>
        [ForeignKey(nameof(CategoryId))]
        public virtual CategoryDbo Category { get; set; } = null!;

        /// <summary>
        /// Lista rezerwacji powiązana z obiektem.
        /// </summary>
        public virtual ICollection<ReservationDbo>? Reservations { get; set; }

        /// <summary>
        /// Wersja rekordu dla kontroli współbieżności.
        /// </summary>
        [Timestamp]
        public byte[] RowVersion { get; set; } = null!;
    }
}
