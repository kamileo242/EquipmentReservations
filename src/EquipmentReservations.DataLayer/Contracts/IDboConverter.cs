namespace EquipmentReservations.DataLayer
{
    /// <summary>
    /// Zbiór operacji związanych z konwersją danych.
    /// </summary>
    public interface IDboConverter
    {
        /// <summary>
        /// Konwersja jednego obiektu na inny typ za pomocą mappera.
        /// </summary>
        /// <typeparam name="TResult">Typ obiektu wynikowego.</typeparam>
        /// <returns>Utworzony obiekt wynikowy.</returns>
        TResult Convert<TResult>(object source);
    }
}
