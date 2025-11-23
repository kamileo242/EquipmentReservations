using EquipmentReservations.Models;

namespace EquipmentReservations.Domain
{
    /// <summary>
    /// Zbiór operacji wykonywanych na logach audytowych.
    /// </summary>
    public interface IAuditLogService
    {
        /// <summary>
        /// Wyszukuje log autytowy po identyfikatorze.
        /// </summary>
        Task<AuditLog> GetByIdAsync(Guid id);

        /// <summary>
        /// Pobiera wszystkie logi audytowe.
        /// </summary>
        Task<IEnumerable<AuditLog>> GetAllAsync();

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
