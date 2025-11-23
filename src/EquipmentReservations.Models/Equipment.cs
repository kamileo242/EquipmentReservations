namespace EquipmentReservations.Models
{
    /// <summary>
    /// Obiekt. 
    /// </summary>
    public class Equipment
    {
        /// <summary>
        /// Identyfikator obiektu.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Nazwa obiektu.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Opis obiektu.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Identyfikator kategorii do której należy sprzęt.
        /// </summary>
        public Guid CategoryId { get; set; }

        /// <summary>
        /// Kategoria do której należy obiekt.
        /// </summary>
        public Category? Category { get; set; }

        /// <summary>
        /// Lista rezerwacji powiązana z obiektem.
        /// </summary>
        public ICollection<Reservation>? Reservations { get; set; }
    }
}
