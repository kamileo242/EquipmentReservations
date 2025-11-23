namespace EquipmentReservations.Domain
{
    /// <summary>
    /// Zbiór operacji wykonywanych na danych w pamięci podręcznej.
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// Pobiera obiekt z pamięci podręcznej.
        /// </summary>
        /// <typeparam name="T">Typ obiektu do pobrania.</typeparam>
        /// <param name="key">Unikalny klucz identyfikujący dane w pamięci podręcznej.</param>
        /// <returns>Obiekt z pamięci podręcznej lub null, jeśli nie istnieje.</returns>
        T? Get<T>(string key);

        /// <summary>
        /// Zapisuje obiekt do pamięci podręcznej na określony czas.
        /// </summary>
        /// <typeparam name="T">Typ obiektu do zapisania.</typeparam>
        /// <param name="key">Unikalny klucz identyfikujący dane.</param>
        /// <param name="value">Wartość do zapisania.</param>
        /// <param name="duration">Czas ważności danych w pamięci podręcznej.</param>
        void Set<T>(string key, T value, TimeSpan duration);

        /// <summary>
        /// Usuwa dane z pamięci podręcznej dla podanego klucza.
        /// </summary>
        /// <param name="key">Klucz identyfikujący dane do usunięcia.</param>
        void Remove(string key);
    }
}
