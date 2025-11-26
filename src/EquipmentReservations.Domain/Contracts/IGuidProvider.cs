namespace EquipmentReservations.Domain
{
  /// <summary>
  /// Interefejs do nadawania identyfikatorów dla nowych obiektów.
  /// </summary>
  public interface IGuidProvider
  {
    /// <summary>
    /// Generuje nowy Guid.
    /// </summary>
    /// <returns>Identyfikator nowego obiektu.</returns>
    Guid GenerateGuid();
  }
}
