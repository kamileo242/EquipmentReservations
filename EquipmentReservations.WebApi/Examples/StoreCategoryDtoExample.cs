using EquipmentReservations.WebApi.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace EquipmentReservations.WebApi.Examples
{
    /// <summary>
    /// Przykład danych wejściowych dla tworzenia lub aktualizacji kategorii.
    /// </summary>
    public class StoreCategoryDtoExample : IExamplesProvider<StoreCategoryDto>
    {
        public StoreCategoryDto GetExamples()
            => new()
            {
                Name = "Laptopy",
                ParentCategoryId = "00000000000000000000000000000001"
            };
    }
}
