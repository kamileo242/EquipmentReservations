using EquipmentReservations.WebApi.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace EquipmentReservations.WebApi.Examples
{
    /// <summary>
    /// Przykład listy sprzętu.
    /// </summary>
    public class EquipmentDtoListExample : IExamplesProvider<IEnumerable<EquipmentDto>>
    {
        public IEnumerable<EquipmentDto> GetExamples()
            => new List<EquipmentDto>
            {
                new() {
                    Id = "00000000000000000000000000000004",
                    Name = "HP EliteBook 840 G9",
                    Description = "Ultrabook biznesowy z 32GB RAM i SSD 1TB.",
                    CategoryId = "00000000000000000000000000000002",
                    CategoryName = "Laptopy"
                },
                new() {
                    Id = "00000000000000000000000000000005",
                    Name = "iPhone 15 Pro",
                    Description = "Smartfon firmowy z systemem iOS.",
                    CategoryId = "00000000000000000000000000000003",
                    CategoryName = "Telefony"
                }
            };
    }
}
