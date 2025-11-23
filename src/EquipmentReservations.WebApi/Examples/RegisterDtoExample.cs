using EquipmentReservations.WebApi.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace EquipmentReservations.WebApi.Examples
{
    /// <summary>
    /// Przykład pojedynczego wpisu przy rejestracji użytkownika.
    /// </summary>
    public class RegisterDtoExample : IExamplesProvider<RegisterDto>
    {
        public RegisterDto GetExamples()
        {
            return new RegisterDto
            {
                Email = "jan.kowalski@example.com",
                Password = "Test123!",
                FullName = "Jan Kowalski"
            };
        }
    }
}
