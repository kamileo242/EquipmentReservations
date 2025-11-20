using System.ComponentModel.DataAnnotations;

namespace EquipmentReservations.DataLayer.Dbos
{
    /// <summary>
    /// Reprezentacja loga audytowego w bazie danch 
    /// </summary>
    public class AuditLogDbo
    {
        /// <summary>
        /// Identyfikator.
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Nazwa encji, której dotyczy zmiana.
        /// </summary>
        [Required]
        public string EntityName { get; set; } = null!;

        /// <summary>
        /// Identyfikator encji, której dotyczy zmiana.
        /// </summary>
        [Required]
        public string EntityId { get; set; } = null!;

        /// <summary>
        /// Rodzaj wykonanej akcji.
        /// </summary>
        [Required]
        public string Action { get; set; } = null!;

        /// <summary>
        /// Json zawierający szczegóły zmian.
        /// </summary>
        [Required]
        public string ChangeJson { get; set; } = null!;

        /// <summary>
        /// Użytkownik, który dokonał zmiany.
        /// </summary>
        [Required]
        public string ChangedBy { get; set; } = null!;

        /// <summary>
        /// Czas dokonania zmiany.
        /// </summary>
        public DateTime ChangedAt { get; set; }
    }
}
