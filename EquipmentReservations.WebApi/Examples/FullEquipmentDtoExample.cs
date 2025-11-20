using EquipmentReservations.WebApi.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace EquipmentReservations.WebApi.Examples
{
    /// <summary>
    /// Przykładowy obiekt <see cref="FullEquipmentDto"/> prezentowany w dokumentacji Swaggera.
    /// </summary>
    public class FullEquipmentDtoExample : IExamplesProvider<FullEquipmentDto>
    {
        public FullEquipmentDto GetExamples()
            => new()
            {
                Id = "e7b4f7a85b774b27a1e8b28b1d7d5115",
                Name = "Projektor multimedialny Epson EB-X41",
                Description = "Projektor z wysoką jasnością, idealny do prezentacji i szkoleń.",
                Category = new CategoryDto
                {
                    Id = "c5a6e39d6f9248dc9b4439a11c8c4b1e",
                    Name = "Sprzęt audiowizualny"
                },
                Reservations = new List<ReservationDto>
                {
                    new()
                    {
                        Id = "f3c9f24bd8b34d8cb4f19df1d70e3199",
                        EquipmentId = "e7b4f7a85b774b27a1e8b28b1d7d5115",
                        EquipmentName = "Projektor multimedialny Epson EB-X41",
                        UserId = "5f5f77bb0f5f499ebb9edfa4bb4ff555",
                        Start = DateTime.UtcNow.AddDays(2),
                        End = DateTime.UtcNow.AddDays(4),
                        ReservationStatus = "Confirmed",
                        CreatedAt = DateTime.UtcNow.AddDays(-1),
                        UpdatedAt = DateTime.UtcNow.AddHours(-2)
                    },
                    new()
                    {
                        Id = "2d96a6d2435048ada5ff3c94e4debb22",
                        EquipmentId = "e7b4f7a85b774b27a1e8b28b1d7d5115",
                        EquipmentName = "Projektor multimedialny Epson EB-X41",
                        UserId = "a1b3d9f499444a6f9e8fcb2a78a0c3e2",
                        Start = DateTime.UtcNow.AddDays(10),
                        End = DateTime.UtcNow.AddDays(12),
                        ReservationStatus = "Pending",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    }
                }
            };
    }
}
