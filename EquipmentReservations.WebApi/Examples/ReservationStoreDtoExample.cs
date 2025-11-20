using EquipmentReservations.WebApi.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace EquipmentReservations.WebApi.Examples
{
    /// <summary>
    /// Przykład danych wejściowych dla tworzenia rezerwacji.
    /// </summary>
    public class ReservationStoreDtoExample : IExamplesProvider<ReservationStoreDto>
    {
        public ReservationStoreDto GetExamples()
            => new()
            {
                EquipmentId = "00000000000000000000000000000003",
                UserId = "user123",
                Start = DateTime.UtcNow.AddHours(1),
                End = DateTime.UtcNow.AddHours(4)
            };
    }
}
