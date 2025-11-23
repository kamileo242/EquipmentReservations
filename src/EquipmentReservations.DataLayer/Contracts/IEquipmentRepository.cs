using EquipmentReservations.Models;

namespace EquipmentReservations.DataLayer
{
    /// <summary>
    /// Zbiór operacji wykonywanych na repozytorium wypożycznaego obiektu.
    /// </summary>
    public interface IEquipmentRepository : IRepository<Equipment>
    {
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
        Task<Equipment?> GetFullEquipmentAsync(Guid id);

        /// <summary>
        /// Pobiera sprzęt w sposób stronicowany.
        /// </summary>
        Task<IEnumerable<Equipment>> GetPagedAsync(int pageNumber, int pageSize);
    }
}
