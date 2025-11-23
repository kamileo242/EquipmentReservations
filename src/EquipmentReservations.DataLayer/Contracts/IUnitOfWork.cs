namespace EquipmentReservations.DataLayer
{
    /// <summary>
    /// Interfejs zawierający zbiór operacji zapewniających spójność transakcji między repozytoriami.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Interfejs repozytorium rezerwacji.
        /// </summary>
        IReservationRepository Reservations { get; }

        /// <summary>
        /// Interfejs repozytorium obiektu do wypożyczenia.
        /// </summary>
        IEquipmentRepository Equipments { get; }

        /// <summary>
        /// Interfejs repozytorium kategorii.
        /// </summary>
        ICategoryRepository Categories { get; }

        /// <summary>
        /// Interfejs repozytorium logów audytowych.
        /// </summary>
        IAuditLogRepository AuditLogs { get; }

        /// <summary>
        /// Zatwierdza wszystkie zmiany w ramach bieżącej transakcji.
        /// </summary>
        Task<int> SaveChangesAsync();
    }
}
