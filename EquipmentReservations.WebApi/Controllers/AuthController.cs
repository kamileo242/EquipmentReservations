using System.Net.Mime;
using EquipmentReservations.Domain;
using EquipmentReservations.Models;
using EquipmentReservations.WebApi.Dtos;
using EquipmentReservations.WebApi.Examples;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace EquipmentReservations.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public const string RegisterUser = "Rejestruje użytkownika.";
        public const string LoginUser = "Logowanie użytkownika.";

        private readonly UserManager<ApplicationUser> userManager;
        private readonly IJwtTokenGenerator jwtTokenGenerator;

        public AuthController(UserManager<ApplicationUser> userManager, IJwtTokenGenerator jwtTokenGenerator)
        {
            this.userManager = userManager;
            this.jwtTokenGenerator = jwtTokenGenerator;
        }

        [HttpPost("register")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(RegisterDtoExample))]
        [SwaggerOperation(RegisterUser)]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FullName = dto.FullName
            };

            var result = await userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await userManager.AddToRoleAsync(user, "User");

            return Ok("Użytkownik zarejestrowany.");
        }

        [HttpPost("login")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [SwaggerOperation("LoginUser")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await userManager.FindByEmailAsync(dto.Email);

            if (user == null || !await userManager.CheckPasswordAsync(user, dto.Password))
                return Unauthorized("Niepoprawne dane logowania.");

            var roles = await userManager.GetRolesAsync(user);

            var token = jwtTokenGenerator.GenerateToken(
                user.Id,
                user.Email!,
                user.UserName!,
                roles
            );

            return Ok(token);
        }
    }
}
