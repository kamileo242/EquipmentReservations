using EquipmentReservations.WebApi.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace EquipmentReservations.WebApi.Examples
{
    /// <summary>
    /// Przykład pojedynczego wpisu przy logowaniu użytkownika.
    /// </summary>
    public class LoginDtoExample : IExamplesProvider<LoginDto>
    {
        public LoginDto GetExamples()
        {
            return new LoginDto
            {
                Email = "jan.kowalski@example.com",
                Password = "Test123!"
            };
        }
    }
}