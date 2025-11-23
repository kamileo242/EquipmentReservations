using EquipmentReservations.WebApi.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace EquipmentReservations.WebApi.Examples
{
    /// <summary>
    /// Przykład pojedynczej kategorii.
    /// </summary>
    public class CategoryDtoExample : IExamplesProvider<CategoryDto>
    {
        public CategoryDto GetExamples()
            => new()
            {
                Id = "00000000000000000000000000000001",
                Name = "Elektronika"
            };
    }
}
