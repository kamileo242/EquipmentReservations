using EquipmentReservations.Models;

namespace EquipmentReservations.DataLayer
{
    /// <summary>
    /// Zbiór operacji wykonywanych na repozytorium logów audytowych.
    /// </summary>
    public interface IAuditLogRepository : IRepository<AuditLog>
    {
        /// <summary>
        /// Pobiera najnowsze logi audytowe.
        /// </summary>
        Task<IEnumerable<AuditLog>> GetLatestAsync(int count = 50);

        /// <summary>
        /// Pobiera logi dla konkretnej encji.
        /// </summary>
        Task<IEnumerable<AuditLog>> GetForEntityAsync(string entityName, string entityId);

        /// <summary>
        /// Wyszukuje logi po użytkowniku.
        /// </summary>
        Task<IEnumerable<AuditLog>> SearchAsync(string changedBy);
    }
}
