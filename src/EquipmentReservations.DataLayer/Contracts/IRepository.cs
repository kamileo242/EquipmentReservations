namespace EquipmentReservations.DataLayer
{
    /// <summary>
    /// Generyczny interfejs dla operacji CRUD.
    /// </summary>
    /// <typeparam name="TModel">Model biznesowy</typeparam>
    public interface IRepository<TModel> where TModel : class
    {
        /// <summary>
        /// Pobiera encję po identyfikatorze. 
        /// </summary>
        /// <param name="id">Identyfikator encji</param>
        /// <returns>Znalezionny obiekt na podstawie identyfikatora.</returns>
        Task<TModel> GetByIDAsync(Guid id);

        /// <summary>
        /// Pobiera wszystkie encje danego typu.
        /// </summary>
        /// <returns>Lista wszystkich encji.</returns>
        Task<IEnumerable<TModel>> GetAllAsync();

        /// <summary>
        /// Dodaje nową encję do kontekstu.
        /// </summary>
        /// <param name="entity">Encja do dodania.</param>
        /// <returns>Dodana encja,</returns>
        Task<TModel> AddAsync(TModel entity);

        /// <summary>
        /// Aktualizuje encję w kontekście.
        /// </summary>
        /// <param name="entity">Encja do aktualizacji.</param>
        /// <returns>Zaktualizowany obiekt.</returns>
        Task<TModel> UpdateAsync(TModel entity);

        /// <summary>
        /// Usuwa encję z kontekstu.
        /// </summary>
        /// <param name="entity">Encja do usunięcia.</param>
        Task DeleteAsync(Guid id);
    }
}
