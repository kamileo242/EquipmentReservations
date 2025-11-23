using System.ComponentModel.DataAnnotations;

namespace EquipmentReservations.WebApi.Dtos
{
    /// <summary>
    /// Dane wymagane do zarejestrowania nowego użytkownika.
    /// </summary>
    public record RegisterDto
    {
        /// <summary>
        /// Adres Email
        /// </summary>
        [Required]
        public string Email { get; set; } = null!;

        /// <summary>
        /// Hasło użytkownika.
        /// </summary>
        [Required]
        public string Password { get; set; } = null!;

        /// <summary>
        /// Imię i nazwisko.
        /// </summary>
        [Required]
        public string FullName { get; set; } = null!;
    }
}
