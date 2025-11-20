using EquipmentReservations.WebApi.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace EquipmentReservations.WebApi.Examples
{
    /// <summary>
    /// Przykład pojedynczego wpisu w logu audytu.
    /// </summary>
    public class AuditLogDtoExample : IExamplesProvider<AuditLogDto>
    {
        public AuditLogDto GetExamples()
            => new()
            {
                Id = "00000000000000000000000000000050",
                EntityName = "Equipment",
                EntityId = "00000000000000000000000000000004",
                Action = "Update",
                ChangeJson = "{ \"Name\": \"HP EliteBook 840 G9\" }",
                ChangedBy = "adminUser",
                ChangedAt = DateTime.UtcNow.AddMinutes(-15)
            };
    }
}
