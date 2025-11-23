namespace EquipmentReservations.Models
{
    /// <summary>
    /// Log audytowy zmian w systemie.
    /// </summary>
    public class AuditLog
    {
        /// <summary>
        /// Identyfikator.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Nazwa encji, której dotyczy zmiana.
        /// </summary>
        public string EntityName { get; set; } = null!;

        /// <summary>
        /// Identyfikator encji, której dotyczy zmiana.
        /// </summary>
        public string EntityId { get; set; } = null!;

        /// <summary>
        /// Rodzaj wykonanej akcji.
        /// </summary>
        public string Action { get; set; } = null!;

        /// <summary>
        /// Json zawierający szczegóły zmian.
        /// </summary>
        public string ChangeJson { get; set; } = null!;

        /// <summary>
        /// Użytkownik, który dokonał zmiany.
        /// </summary>
        public string ChangedBy { get; set; } = null!;

        /// <summary>
        /// Czas dokonania zmiany.
        /// </summary>
        public DateTime ChangedAt { get; set; }
    }
}
