using EquipmentReservations.Models;

namespace EquipmentReservations.RabbitMq
{
  /// <summary>
  /// Interfejs definiujący funkcjonalność publikowania informacji 
  /// o rezerwacjach do brokera RabbitMQ.
  /// </summary>
  public interface IReservationRabbitPublisher
  {
    /// <summary>
    /// Publikuje wiadomość zawierającą dane rezerwacji do zdefiniowanego exchange w RabbitMQ, używając ustawień przekazanych w konfiguracji.
    /// </summary>
    /// <param name="reservation">
    /// Model rezerwacji, który zostanie zserializowany i wysłany jako wiadomość.
    /// </param>
    /// <returns>
    /// Zwraca zadanie reprezentujące asynchroniczną operację publikacji komunikatu.
    /// </returns>
    Task PublishReservationAsync(Reservation reservation);
  }
}
