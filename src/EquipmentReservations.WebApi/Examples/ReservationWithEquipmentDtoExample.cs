using EquipmentReservations.WebApi.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace EquipmentReservations.WebApi.Examples
{
    /// <summary>
    /// Przykładowy obiekt <see cref="ReservationWithEquipmentDto"/> prezentowany w dokumentacji Swaggera.
    /// </summary>
    public class ReservationWithEquipmentDtoExample : IExamplesProvider<ReservationWithEquipmentDto>
    {
        public ReservationWithEquipmentDto GetExamples()
            => new()
            {
                Id = "b123e456-7890-1234-5678-9abcdef01234",
                Equipment = new EquipmentDto
                {
                    Id = "e7b4f7a8-5b77-4b27-a1e8-b28b1d7d5115",
                    Name = "Projektor multimedialny Epson EB-X41",
                    Description = "Projektor z wysoką jasnością, idealny do prezentacji i szkoleń.",
                    CategoryId = "c5a6e39d-6f92-48dc-9b44-39a11c8c4b1e"
                },
                UserId = "5f5f77bb-0f5f-499e-bb9e-dfa4bb4ff555",
                Start = DateTime.UtcNow.AddDays(1),
                End = DateTime.UtcNow.AddDays(3),
                ReservationStatus = "Confirmed",
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                UpdatedAt = DateTime.UtcNow
            };
    }
}
