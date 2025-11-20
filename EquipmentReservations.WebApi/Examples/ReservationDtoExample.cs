using EquipmentReservations.WebApi.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace EquipmentReservations.WebApi.Examples
{
    /// <summary>
    /// Przykład pojedynczej rezerwacji.
    /// </summary>
    public class ReservationDtoExample : IExamplesProvider<ReservationDto>
    {
        public ReservationDto GetExamples()
            => new()
            {
                Id = "00000000000000000000000000000010",
                EquipmentId = "00000000000000000000000000000004",
                EquipmentName = "HP EliteBook 840 G9",
                UserId = "user123",
                Start = DateTime.UtcNow.AddHours(1),
                End = DateTime.UtcNow.AddHours(3),
                ReservationStatus = "Active",
                CreatedAt = DateTime.UtcNow.AddMinutes(-30),
                UpdatedAt = DateTime.UtcNow.AddMinutes(-10)
            };
    }
}
