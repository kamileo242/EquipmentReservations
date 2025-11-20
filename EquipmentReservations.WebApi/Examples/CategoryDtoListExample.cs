using EquipmentReservations.WebApi.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace EquipmentReservations.WebApi.Examples
{
    /// <summary>
    /// Przykład listy kategorii.
    /// </summary>
    public class CategoryDtoListExample : IExamplesProvider<IEnumerable<CategoryDto>>
    {
        public IEnumerable<CategoryDto> GetExamples()
            => new List<CategoryDto>
            {
                new() { Id = "00000000000000000000000000000001", Name = "Elektronika" },
                new() { Id = "00000000000000000000000000000002", Name = "Laptopy" },
                new() { Id = "00000000000000000000000000000003", Name = "Telefony" }
            };
    }

}
