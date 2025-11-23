using EquipmentReservations.WebApi.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace EquipmentReservations.WebApi.Examples
{
    /// <summary>
    /// Przykład listy rezerwacji.
    /// </summary>
    public class ReservationDtoListExample : IExamplesProvider<IEnumerable<ReservationDto>>
    {
        public IEnumerable<ReservationDto> GetExamples()
            => new List<ReservationDto>
            {
                new() {
                    Id = "00000000000000000000000000000010",
                    EquipmentId = "00000000000000000000000000000004",
                    EquipmentName = "HP EliteBook 840 G9",
                    UserId = "user123",
                    Start = DateTime.UtcNow.AddHours(1),
                    End = DateTime.UtcNow.AddHours(3),
                    ReservationStatus = "Active",
                    CreatedAt = DateTime.UtcNow.AddMinutes(-30),
                    UpdatedAt = DateTime.UtcNow.AddMinutes(-10)
                },
                new() {
                    Id = "00000000000000000000000000000011",
                    EquipmentId = "00000000000000000000000000000005",
                    EquipmentName = "iPhone 15 Pro",
                    UserId = "adminUser",
                    Start = DateTime.UtcNow.AddDays(1),
                    End = DateTime.UtcNow.AddDays(2),
                    ReservationStatus = "Pending",
                    CreatedAt = DateTime.UtcNow.AddMinutes(-60),
                    UpdatedAt = DateTime.UtcNow.AddMinutes(-5)
                }
            };
    }
}
