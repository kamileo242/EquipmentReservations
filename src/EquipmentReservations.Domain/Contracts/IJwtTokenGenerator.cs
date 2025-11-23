namespace EquipmentReservations.Domain
{
    /// <summary>
    /// Generuje tokeny JWT dla zalogowanych użytkowników.
    /// </summary>
    public interface IJwtTokenGenerator
    {
        /// <summary>
        /// Tworzy token JWT na podstawie danych użytkownika.
        /// </summary>
        /// <param name="userId">Identyfikator użytkownika.</param>
        /// <param name="email">Email użytkownika.</param>
        /// <param name="userName">Nazwa użytkownika.</param>
        /// <param name="roles">Lista ról użytkownika.</param>
        /// <returns>Wygenerowany token JWT.</returns>
        string GenerateToken(string userId, string email, string userName, IList<string> roles);
    }
}