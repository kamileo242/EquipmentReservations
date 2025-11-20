using EquipmentReservations.Models;

namespace EquipmentReservations.DataLayer
{
    /// <summary>
    /// Zbiór operacji wykonywanych na repozytorium rezerwacji.
    /// </summary>
    public interface IReservationRepository : IRepository<Reservation>
    {
        /// <summary>
        /// Pobiera wszystkie rezerwacje użytkownika.
        /// </summary>
        Task<IEnumerable<Reservation>> GetByUserAsync(string userId);

        /// <summary>
        /// Sprawdza czy obiekt jest dostępny w danym zakresie czasu.
        /// </summary>
        Task<bool> IsEquipmentAvailableAsync(Guid equipmentId, DateTime start, DateTime end);

        /// <summary>
        /// Pobiera aktywne rezerwacje.
        /// </summary>
        Task<IEnumerable<Reservation>> GetActiveAsync();

        /// <summary>
        /// Wyszukuje rezerwacje po nazwie sprzętu.
        /// </summary>
        Task<IEnumerable<Reservation>> SearchAsync(string phrase);
    }
}
