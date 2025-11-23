using Microsoft.AspNetCore.Identity;

namespace EquipmentReservations.Models
{
    /// <summary>
    /// Reprezentuje użytkownika systemu.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>Pełne imię i nazwisko użytkownika.</summary>
        public string? FullName { get; set; }
    }
}
