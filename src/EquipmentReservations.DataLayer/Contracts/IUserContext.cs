namespace EquipmentReservations.DataLayer
{
    /// <summary>
    /// Zapewnia dostęp do informacji o aktualnie zalogowanym użytkowniku.
    /// </summary>
    public interface IUserContext
    {
        /// <summary>
        /// Zwraca identyfikator aktualnie zalogowanego użytkownika (lub null jeśli niezalogowany).
        /// </summary>
        string? GetCurrentUser();

        /// <summary>
        /// Zwraca role zalogowanego użytkownika.
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetUserRoles();
    }
}
