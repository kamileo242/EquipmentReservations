using EquipmentReservations.WebApi.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace EquipmentReservations.WebApi.Examples
{
    /// <summary>
    /// Przykład danych wejściowych dla tworzenia sprzętu.
    /// </summary>
    public class StoreEquipmentDtoExample : IExamplesProvider<StoreEquipmentDto>
    {
        public StoreEquipmentDto GetExamples()
            => new()
            {
                Name = "Dell Latitude 5440",
                Description = "Laptop biurowy z procesorem Intel i7 i 16GB RAM.",
                CategoryId = "00000000000000000000000000000002"
            };
    }
}
