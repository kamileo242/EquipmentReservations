using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EquipmentReservations.DataLayer.Dbos
{
    /// <summary>
    /// Reprezentacja rezerwacji w bazie danych.
    /// </summary>
    public class ReservationDbo
    {
        /// <summary>
        /// Identyfikator.
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Idenntyfikator zarezerwowanego obiektu.
        /// </summary>
        [Required]
        public Guid EquipmentId { get; set; }

        /// <summary>
        /// Reprezentacja Zarezerwowanego obiektu w bazie danych.
        /// </summary>
        [ForeignKey(nameof(EquipmentId))]
        public EquipmentDbo Equipment { get; set; } = null!;

        /// <summary>
        /// Identyfikator użytkownika.
        /// </summary>
        [Required]
        public string UserId { get; set; } = null!;

        /// <summary>
        /// Data i godzina rozpoczęcia rezerwacji
        /// </summary>
        public DateTime Start { get; set; }

        /// <summary>
        /// Data i godzina zakończenia rezerwacji.
        /// </summary>
        public DateTime? End { get; set; }

        /// <summary>
        /// Status rezerwacji.
        /// </summary>
        [Required]
        public string ReservationStatus { get; set; } = null!;

        /// <summary>
        /// Data utworzenia rezerwacji.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Data ostatniej modyfikacji rezerwacji.
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Wersja rekordu dla kontroli współbieżności.
        /// </summary>
        [Timestamp]
        public byte[] RowVersion { get; set; } = null!;
    }
}
