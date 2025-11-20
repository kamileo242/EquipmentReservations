using EquipmentReservations.DataLayer;
using EquipmentReservations.Models;
using EquipmentReservations.Models.Exceptions;

namespace EquipmentReservations.Domain.Services
{
    public class AuditLogService : IAuditLogService
    {
        private readonly IAuditLogRepository repository;

        public AuditLogService(IAuditLogRepository repository)
        {
            this.repository = repository;
        }

        public async Task<AuditLog> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id nie może być pustym GUID-em.", nameof(id));

            var audit = await repository.GetByIDAsync(id);
            if (audit == null)
                throw new MissingDataException($"Nie znaleziono loga audytowego o id: {id}");

            return audit;
        }

        public async Task<IEnumerable<AuditLog>> GetAllAsync()
        {
            return await repository.GetAllAsync();
        }

        public async Task<IEnumerable<AuditLog>> GetForEntityAsync(string entityName, string entityId)
        {
            if (string.IsNullOrEmpty(entityName))
                throw new ArgumentNullException(nameof(entityName));

            if (string.IsNullOrEmpty(entityId))
                throw new ArgumentNullException(nameof(entityId));

            return await repository.GetForEntityAsync(entityName, entityId);
        }

        public async Task<IEnumerable<AuditLog>> GetLatestAsync(int count = 50)
        {
            return await repository.GetLatestAsync();
        }

        public async Task<IEnumerable<AuditLog>> SearchAsync(string changedBy)
        {
            if (string.IsNullOrEmpty(changedBy))
                throw new ArgumentNullException(nameof(changedBy));

            return await repository.SearchAsync(changedBy);
        }
    }
}
