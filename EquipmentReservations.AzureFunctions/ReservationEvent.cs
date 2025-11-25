namespace EquipmentReservations.AzureFunctions
{
  /// <summary>
  /// Rezerwacja obiektu.
  /// </summary>
  public class ReservationEvent
  {
    /// <summary>
    /// Identyfikator.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Identyfikator zarezerwowanego obiektu.
    /// </summary>
    public Guid EquipmentId { get; set; }

    /// <summary>
    /// Identyfikator użytkownika.
    /// </summary>
    public string UserId { get; set; } = null!;

    /// <summary>
    /// Data i godzina rozpoczęcia rezerwacji
    /// </summary>
    public DateTime Start { get; set; }

    /// <summary>
    /// Data i godzina zakończenia rezerwacji.
    /// </summary>
    public DateTime? End { get; set; }

    /// <summary>
    /// Status rezerwacji.
    /// </summary>
    public string ReservationStatus { get; set; } = null!;

    /// <summary>
    /// Wykonywana akcja (Created/Updated).
    /// </summary>
    public string Action { get; set; } = null!;

    /// <summary>
    /// Data wysłania rezerwacji.
    /// </summary>
    public DateTime OccurredAt { get; set; }
  }
}
