using EquipmentReservations.Models;

namespace EquipmentReservations.Domain
{
    /// <summary>
    /// Zbiór operacji wykonywanych na serwisie sprzęt do wypożyczenia.
    /// </summary>
    public interface IEquipmentService
    {
        /// <summary>
        /// Wyszukuje sprzęt po identyfikatorze.
        /// </summary>
        Task<Equipment> GetByIdAsync(Guid id);

        /// <summary>
        /// Pobiera wszystkie sprzęty.
        /// </summary>
        Task<IEnumerable<Equipment>> GetAllAsync();

        /// <summary>
        /// Dodaje nowy sprzęt.
        /// </summary>
        Task<Equipment> AddAsync(Equipment equipment);

        /// <summary>
        /// Aktualizuje istniejący sprzęt.
        /// </summary>
        Task<Equipment> UpdateAsync(Equipment equipment);

        /// <summary>
        /// Usuwa sprzęt po identyfikatorze.
        /// </summary>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Pobiera sprzęt należący do danej kategorii.
        /// </summary>
        Task<IEnumerable<Equipment>> GetByCategoryAsync(Guid categoryId);

        /// <summary>
        /// Wyszukuje sprzęt po nazwie lub opisie.
        /// </summary>
        Task<IEnumerable<Equipment>> SearchAsync(string phrase);

        /// <summary>
        /// Pobiera sprzęt wraz z informacjami o kategoriach i rezerwacjach.
        /// </summary>
        Task<Equipment> GetFullEquipmentAsync(Guid id);

        /// <summary>
        /// Pobiera sprzęt w sposób stronicowany.
        /// </summary>
        Task<IEnumerable<Equipment>> GetPagedAsync(int pageNumber, int pageSize);
    }
}
