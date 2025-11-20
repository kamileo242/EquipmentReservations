using EquipmentReservations.Models;

namespace EquipmentReservations.Domain
{
    /// <summary>
    /// Zbiór operacji wykonywanych na serwisie rezerwacji.
    /// </summary>
    public interface IReservationService
    {
        /// <summary>
        /// Wyszukuje rezerwacje po identyfikatorze.
        /// </summary>
        Task<Reservation> GetByIdAsync(Guid id);

        /// <summary>
        /// Pobiera wszystkie rezerwacje.
        /// </summary>
        Task<IEnumerable<Reservation>> GetAllAsync();

        /// <summary>
        /// Dodaje nową rezerwację.
        /// </summary>
        Task<Reservation> AddAsync(Reservation reservation);

        /// <summary>
        /// Aktualizuje istniejącą rezerwację.
        /// </summary>
        Task<Reservation> UpdateAsync(Reservation reservation);

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
