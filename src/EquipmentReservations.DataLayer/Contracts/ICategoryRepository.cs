using EquipmentReservations.Models;

namespace EquipmentReservations.DataLayer
{
    /// <summary>
    /// Zbiór operacji wykonywanych na repozytorium kategorii. 
    /// </summary>
    public interface ICategoryRepository : IRepository<Category>
    {
        /// <summary>
        /// Pobiera wszystkie kategorie nadrzędne (bez rodziców).
        /// </summary>
        Task<IEnumerable<Category>> GetRootCategoriesAsync();

        /// <summary>
        /// Pobiera drzewo kategorii wraz z podkategoriami.
        /// </summary>
        Task<Category?> GetCategoryTreeAsync(Guid categoryId);

        /// <summary>
        /// Wyszukuje kategorie po nazwie.
        /// </summary>
        Task<IEnumerable<Category>> SearchAsync(string phrase);
    }
}
