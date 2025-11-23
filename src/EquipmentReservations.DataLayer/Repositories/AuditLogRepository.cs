using EquipmentReservations.DataLayer.Dbos;
using EquipmentReservations.Models;
using Microsoft.EntityFrameworkCore;

namespace EquipmentReservations.DataLayer.Repositories
{
    public class AuditLogRepository : Repository<AuditLog, AuditLogDbo>, IAuditLogRepository
    {
        private readonly EquipmentReservationsDbContext context;
        private readonly IDboConverter converter;
        public AuditLogRepository(EquipmentReservationsDbContext context, IDboConverter converter)
            : base(context, converter)
        {
            this.context = context;
            this.converter = converter;
        }

        public async Task<IEnumerable<AuditLog>> GetForEntityAsync(string entityName, string entityId)
        {
            var dbos = await context.AuditLogs
                .Where(l => l.EntityName == entityName && l.EntityId == entityId)
                .OrderByDescending(l => l.ChangedAt)
                .ToListAsync();

            return dbos.Select(converter.Convert<AuditLog>);

        }

        public async Task<IEnumerable<AuditLog>> GetLatestAsync(int count = 50)
        {
            var dbos = await context.AuditLogs
                .OrderByDescending(l => l.ChangedAt)
                .Take(count)
                .ToListAsync();

            return dbos.Select(converter.Convert<AuditLog>);
        }

        public async Task<IEnumerable<AuditLog>> SearchAsync(string changedBy)
        {
            var dbos = await context.AuditLogs
                .Where(l => l.ChangedBy.Contains(changedBy))
                .OrderByDescending(l => l.ChangedAt)
                .ToListAsync();

            return dbos.Select(converter.Convert<AuditLog>);
        }
    }
}
