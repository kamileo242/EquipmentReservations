namespace EquipmentReservations.WebApi.Dtos
{
    /// <summary>
    /// Reprezentuje log audytu zmian w systemie.
    /// </summary>
    public record AuditLogDto
    {
        /// <summary>
        /// Identyfikator rekordu audytu.
        /// </summary>
        public string Id { get; init; } = string.Empty;

        /// <summary>
        /// Nazwa encji, której dotyczy zmiana.
        /// </summary>
        public string EntityName { get; init; } = string.Empty;

        /// <summary>
        /// Identyfikator encji, której dotyczy zmiana.
        /// </summary>
        public string EntityId { get; init; } = string.Empty;

        /// <summary>
        /// Rodzaj wykonanej akcji.
        /// </summary>
        public string Action { get; init; } = string.Empty;

        /// <summary>
        /// Szczegóły zmian w formacie JSON.
        /// </summary>
        public string ChangeJson { get; init; } = string.Empty;

        /// <summary>
        /// Użytkownik, który dokonał zmiany.
        /// </summary>
        public string ChangedBy { get; init; } = string.Empty;

        /// <summary>
        /// Czas dokonania zmiany.
        /// </summary>
        public DateTime ChangedAt { get; init; }
    }
}
