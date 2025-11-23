using EquipmentReservations.DataLayer;
using EquipmentReservations.Models;
using EquipmentReservations.Models.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace EquipmentReservations.Domain.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository reservationRepository;
        private readonly IEquipmentRepository equipmentRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ITimeProvider timeProvider;
        private readonly IUserContext userContext;

        public ReservationService(IReservationRepository reservationRepository,
            IEquipmentRepository equipmentRepository,
            IUnitOfWork unitOfWork,
            ITimeProvider timeProvider,
            IUserContext userContext)
        {
            this.reservationRepository = reservationRepository;
            this.equipmentRepository = equipmentRepository;
            this.unitOfWork = unitOfWork;
            this.timeProvider = timeProvider;
            this.userContext = userContext;
        }

        public async Task<Reservation> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id nie może być pustym GUID-em.", nameof(id));

            var reservation = await reservationRepository.GetByIDAsync(id);
            if (reservation == null)
                throw new MissingDataException($"Nie znaleziono rezerwacji o id: {id}");

            return reservation;
        }

        public async Task<IEnumerable<Reservation>> GetAllAsync()
        {
            return await reservationRepository.GetAllAsync();
        }

        public async Task<Reservation> AddAsync(Reservation reservation)
        {
            if (reservation == null)
                throw new ArgumentNullException(nameof(reservation));

            if (string.IsNullOrEmpty(reservation.UserId))
                throw new IncorrectDataException("Nie podano użytkownika dla którego sprzęt ma być wypożyczony.");

            if (reservation.Start == null || reservation.Start == DateTime.MinValue)
                throw new IncorrectDataException("Nie podano daty początkowej rezerwacji.");

            if (reservation.End > reservation.Start)
                throw new IncorrectDataException("Data zakończenia rezerwacji występuje przed datą jej rozpoczęcia.");

            if (reservation.EquipmentId == Guid.Empty)
                throw new IncorrectDataException("Nie podano sprzętu do wypożyczenia.");

            var isAvailable = await reservationRepository.IsEquipmentAvailableAsync(reservation.Id, reservation.Start, reservation.End.GetValueOrDefault());
            if (!isAvailable)
                throw new IncorrectDataException("Podany sprzęt jest już zarezerwowany w tym tym terminie.");
            var equipment = await equipmentRepository.GetByIDAsync(reservation.EquipmentId);

            if (equipment == null)
                throw new MissingDataException($"Nie znaleziono sprzętu o id: {reservation.EquipmentId}");
            reservation.Equipment = equipment;
            reservation.CreatedAt = timeProvider.GetDateTime();
            reservation.UpdatedAt = timeProvider.GetDateTime();
            reservation.ReservationStatus = ReservationStatus.Pending;

            var result = await reservationRepository.AddAsync(reservation);
            await unitOfWork.SaveChangesAsync();

            return result;
        }

        public async Task<Reservation> UpdateAsync(Reservation reservation)
        {
            if (reservation == null)
                throw new ArgumentNullException(nameof(reservation));

            var existingReservation = await reservationRepository.GetByIDAsync(reservation.Id);
            if (existingReservation == null)
                throw new MissingDataException($"Nie znaleziono rezerwacji o id: {reservation.Id}");

            if (reservation.Start == null || reservation.Start == DateTime.MinValue)
                reservation.Start = existingReservation.Start;

            if (reservation.End == null || reservation.End == DateTime.MinValue)
                reservation.End = existingReservation.End;

            if (reservation.EquipmentId != Guid.Empty)
            {
                var equipment = await equipmentRepository.GetByIDAsync(reservation.EquipmentId);
                if (equipment == null)
                    throw new MissingDataException($"Nie znaleziono sprzętu o id: {reservation.EquipmentId}");
                var isAvailable = await reservationRepository.IsEquipmentAvailableAsync(reservation.Id, reservation.Start, reservation.End.GetValueOrDefault());
                if (!isAvailable)
                    throw new IncorrectDataException("Podany sprzęt jest już zarezerwowany w tym tym terminie.");

                reservation.Equipment = equipment;
            }

            if (reservation.End > reservation.Start)
                throw new IncorrectDataException("Data zakończenia rezerwacji występuje przed datą jej rozpoczęcia.");

            if (existingReservation.ReservationStatus == ReservationStatus.Approved)
            {
                var roles = userContext.GetUserRoles();
                if (!roles.Contains("Admin"))
                {
                    throw new IncorrectDataException("Tylko administrator może potwierdzać rezerwacje.");
                }
            }
            if (existingReservation.ReservationStatus == ReservationStatus.Returned)
            {
                throw new IncorrectDataException("Nie można zmieniać zakończonej rezerwacji.");
            }
            if (existingReservation.ReservationStatus == ReservationStatus.Cancelled)
            {
                throw new IncorrectDataException("Nie można zmieniać anulowanej rezerwacji.");
            }

            reservation.UpdatedAt = timeProvider.GetDateTime();

            try
            {
                var result = await reservationRepository.UpdateAsync(reservation);
                await unitOfWork.SaveChangesAsync();
                return result;
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new ConcurrencyConflictException("Rezerwacja została zmodyfikowana przez innego użytkownika. Odśwież dane i spróbuj ponownie.");
            }
        }

        public async Task<IEnumerable<Reservation>> GetActiveAsync()
        {
            return await reservationRepository.GetActiveAsync();
        }

        public async Task<IEnumerable<Reservation>> GetByUserAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));

            return await reservationRepository.GetByUserAsync(userId);
        }

        public async Task<bool> IsEquipmentAvailableAsync(Guid equipmentId, DateTime start, DateTime end)
        {
            if (equipmentId == Guid.Empty)
                throw new ArgumentException("Id nie może być pustym GUID-em.", nameof(equipmentId));

            if (start == null || start == DateTime.MinValue)
                throw new IncorrectDataException("Nie podano daty początkowej rezerwacji.");

            if (end == null || end == DateTime.MinValue)
                throw new IncorrectDataException("Nie podano daty końcowej rezerwacji.");

            return await reservationRepository.IsEquipmentAvailableAsync(equipmentId, start, end);
        }

        public async Task<IEnumerable<Reservation>> SearchAsync(string phrase)
        {
            if (string.IsNullOrEmpty(phrase))
                throw new ArgumentNullException(nameof(phrase));

            return await reservationRepository.SearchAsync(phrase);
        }
    }
}
