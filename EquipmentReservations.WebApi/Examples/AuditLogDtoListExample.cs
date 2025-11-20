using EquipmentReservations.WebApi.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace EquipmentReservations.WebApi.Examples
{
    /// <summary>
    /// Przykład listy wpisów logów audytu.
    /// </summary>
    public class AuditLogDtoListExample : IExamplesProvider<IEnumerable<AuditLogDto>>
    {
        public IEnumerable<AuditLogDto> GetExamples()
            => new List<AuditLogDto>
            {
                new() {
                    Id = "00000000000000000000000000000050",
                    EntityName = "Equipment",
                    EntityId = "00000000000000000000000000000004",
                    Action = "Update",
                    ChangeJson = "{ \"Description\": \"Zmieniono opis sprzętu.\" }",
                    ChangedBy = "adminUser",
                    ChangedAt = DateTime.UtcNow.AddMinutes(-15)
                },
                new() {
                    Id = "00000000000000000000000000000051",
                    EntityName = "Category",
                    EntityId = "00000000000000000000000000000002",
                    Action = "Add",
                    ChangeJson = "{ \"Name\": \"Laptopy\" }",
                    ChangedBy = "system",
                    ChangedAt = DateTime.UtcNow.AddHours(-2)
                }
            };
    }
}
