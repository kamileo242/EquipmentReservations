namespace EquipmentReservations.Models
{
    /// <summary>
    /// Rezerwacja obiektu.
    /// </summary>
    public class Reservation
    {
        /// <summary>
        /// Identyfikator.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Identyfikator zarezerwowanego obiektu.
        /// </summary>
        public Guid EquipmentId { get; set; }

        /// <summary>
        /// Zarezerwowany obiekt.
        /// </summary>
        public Equipment? Equipment { get; set; }

        /// <summary>
        /// Identyfikator użytkownika.
        /// </summary>
        public string UserId { get; set; } = null!;

        /// <summary>
        /// Data i godzina rozpoczęcia rezerwacji
        /// </summary>
        public DateTime Start { get; set; }

        /// <summary>
        /// Data i godzina zakończenia rezerwacji.
        /// </summary>
        public DateTime? End { get; set; }

        /// <summary>
        /// Status rezerwacji.
        /// </summary>
        public ReservationStatus ReservationStatus { get; set; }

        /// <summary>
        /// Data utworzenia rezerwacji.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Data ostatniej modyfikacji rezerwacji.
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
}
