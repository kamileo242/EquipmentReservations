using EquipmentReservations.Models;

namespace EquipmentReservations.Domain
{
    /// <summary>
    /// Zbiór operacji wykonywanych na serwisie kategorii.
    /// </summary>
    public interface ICategoryService
    {
        /// <summary>
        /// Wyszukuje kategorie po identyfikatorze.
        /// </summary>
        Task<Category> GetByIdAsync(Guid id);

        /// <summary>
        /// Pobiera wszystkie kategorie.
        /// </summary>
        Task<IEnumerable<Category>> GetAllAsync();

        /// <summary>
        /// Dodaje nową kategorię.
        /// </summary>
        Task<Category> AddAsync(Category category);

        /// <summary>
        /// Aktualizuje istniejącą kategorię.
        /// </summary>
        Task<Category> UpdateAsync(Category category);

        /// <summary>
        /// Usuwa kategorię po identyfikatorze.
        /// </summary>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Pobiera wszystkie kategorie główne (bez kategorii nadrzędnej).
        /// </summary>
        Task<IEnumerable<Category>> GetRootCategoriesAsync();

        /// <summary>
        /// Pobiera drzewo kategorii z uwzględnieniem podkategorii.
        /// </summary>
        Task<Category?> GetCategoryTreeAsync(Guid categoryId);

        /// <summary>
        /// Wyszukuje kategorie po nazwie (fragment).
        /// </summary>
        Task<IEnumerable<Category>> SearchAsync(string phrase);
    }
}
