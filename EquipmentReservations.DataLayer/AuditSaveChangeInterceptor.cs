using EquipmentReservations.DataLayer.Dbos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EquipmentReservations.DataLayer
{
    /// <summary>
    /// Interceptor zapisujący logi audytowe dla operacji EF Core.
    /// </summary>
    public class AuditSaveChangeInterceptor : SaveChangesInterceptor
    {
        private readonly IUserContext userContext;

        public AuditSaveChangeInterceptor(IUserContext userContext)
        {
            this.userContext = userContext;
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            var context = eventData.Context;
            if (context == null) return base.SavingChangesAsync(eventData, result, cancellationToken);

            var entries = context.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added ||
                            e.State == EntityState.Modified ||
                            e.State == EntityState.Deleted);

            foreach (var entry in entries)
            {
                var audit = CreateAuditLog(entry);
                context.Set<AuditLogDbo>().Add(audit);
            }

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private AuditLogDbo CreateAuditLog(EntityEntry entry)
        {
            var entityName = entry.Entity.GetType().Name;
            var action = entry.State.ToString();
            var userId = userContext.GetCurrentUser() ?? "anonymous";

            var changeJson = System.Text.Json.JsonSerializer.Serialize(entry.CurrentValues.ToObject());

            return new AuditLogDbo
            {
                Id = Guid.NewGuid(),
                EntityName = entityName,
                EntityId = entry.Properties.FirstOrDefault(p => p.Metadata.IsPrimaryKey())?.CurrentValue?.ToString() ?? "unknown",
                Action = action,
                ChangedBy = userId,
                ChangeJson = changeJson,
                ChangedAt = DateTime.Now,
            };
        }
    }
}
