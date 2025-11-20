using EquipmentReservations.WebApi.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace EquipmentReservations.WebApi.Examples
{
    /// <summary>
    /// Przykładowe dane drzewa kategorii wykorzystywane w dokumentacji Swagger.
    /// </summary>
    public class CategoryTreeDtoExample : IExamplesProvider<CategoryTreeDto>
    {
        /// <summary>
        /// Zwraca przykładowe drzewo kategorii.
        /// </summary>
        public CategoryTreeDto GetExamples()
            => new()
            {
                Id = "00000000000000000000000000000001",
                Name = "Sprzęt elektroniczny",
                SubCategories = new List<CategoryTreeDto>
                {
                    new()
                    {
                        Id = "00000000000000000000000000000002",
                        Name = "Laptopy",
                        SubCategories = new List<CategoryTreeDto>
                        {
                            new()
                            {
                                Id = "00000000000000000000000000000003",
                                Name = "Ultrabooki"
                            },
                            new()
                            {
                                Id = "00000000000000000000000000000004",
                                Name = "Gamingowe"
                            }
                        }
                    },
                    new()
                    {
                        Id = "00000000000000000000000000000005",
                        Name = "Monitory",
                        SubCategories = new List<CategoryTreeDto>
                        {
                            new()
                            {
                                Id = "00000000000000000000000000000006",
                                Name = "LCD"
                            },
                            new()
                            {
                                Id = "00000000000000000000000000000007",
                                Name = "LED"
                            }
                        }
                    }
                }
            };
    }
}
