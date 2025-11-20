using System.ComponentModel.DataAnnotations;

namespace EquipmentReservations.WebApi.Dtos
{
    /// <summary>
    /// Dane wymagane do logowania.
    /// </summary>
    public record LoginDto
    {
        /// <summary>
        /// Adres e-mail użytkownika.
        /// </summary>
        [Required]
        public string Email { get; set; } = null!;

        /// <summary>
        /// Hasło użytkownika.
        /// </summary>
        [Required]
        public string Password { get; set; } = null!;
    }
}
