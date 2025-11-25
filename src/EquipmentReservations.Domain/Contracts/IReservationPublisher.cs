using EquipmentReservations.Models;

namespace EquipmentReservations.Domain
{
  /// <summary>
  /// Interfejs odpowiedzialny za publikowanie informacji o rezerwacji.
  /// Dzięki temu serwis rezerwacji nie musi znać szczegółów implementacji,
  /// np. wywołań HTTP czy Azure Service Bus.
  /// </summary>
  public interface IReservationPublisher
  {
    /// <summary>
    /// Publikuje informację o rezerwacji.
    /// </summary>
    /// <param name="reservation">Rezerwacja, która została dodana lub zaktualizowana.</param>
    /// <param name="action">Rodzaj akcji: "Created" lub "Updated".</param>
    /// <returns>Task reprezentujący operację asynchroniczną.</returns>
    Task PublishReservationAsync(Reservation reservation, string action);
  }
}
