using EquipmentReservations.WebApi.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace EquipmentReservations.WebApi.Examples
{
    /// <summary>
    /// Przykład pojedynczego sprzętu.
    /// </summary>
    public class EquipmentDtoExample : IExamplesProvider<EquipmentDto>
    {
        public EquipmentDto GetExamples()
            => new()
            {
                Id = "00000000000000000000000000000004",
                Name = "HP EliteBook 840 G9",
                Description = "Ultrabook biznesowy z 32GB RAM i SSD 1TB.",
                CategoryId = "00000000000000000000000000000002",
                CategoryName = "Laptopy"
            };
    }
}
