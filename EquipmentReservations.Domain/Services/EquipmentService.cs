using EquipmentReservations.DataLayer;
using EquipmentReservations.Models;
using EquipmentReservations.Models.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace EquipmentReservations.Domain.Services
{
    public class EquipmentService : IEquipmentService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IEquipmentRepository equipmentRepository;
        private readonly ICategoryRepository categoryRepository;

        public async Task<Equipment> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id nie może być pustym GUID-em.", nameof(id));

            var equipment = await equipmentRepository.GetByIDAsync(id);
            if (equipment == null)
                throw new MissingDataException($"Nie znaleziono sprzętu o id: {id}");

            return equipment;
        }

        public async Task<IEnumerable<Equipment>> GetAllAsync()
        {
            return await equipmentRepository.GetAllAsync();
        }

        public async Task<Equipment> AddAsync(Equipment equipment)
        {
            if (equipment == null)
                throw new ArgumentNullException(nameof(equipment));

            if (string.IsNullOrEmpty(equipment.Name))
                throw new IncorrectDataException("Nazwa sprzętu jest wymagana.");

            if (equipment.CategoryId == Guid.Empty)
                throw new IncorrectDataException("Nie podano kategorii sprzętu.");

            var category = await categoryRepository.GetByIDAsync(equipment.CategoryId);

            if (category == null)
                throw new MissingDataException($"Nie znaleziono kategorii o id: {equipment.CategoryId}");
            equipment.Category = category;

            var result = await equipmentRepository.AddAsync(equipment);
            await unitOfWork.SaveChangesAsync();

            return result;
        }

        public async Task<Equipment> UpdateAsync(Equipment equipment)
        {
            if (equipment == null)
                throw new ArgumentNullException(nameof(equipment));

            if (equipment.CategoryId != Guid.Empty)
            {
                var category = await categoryRepository.GetByIDAsync(equipment.CategoryId);

                if (category == null)
                    throw new MissingDataException($"Nie znaleziono kategorii o id: {equipment.CategoryId}");
            }

            try
            {
                var result = await equipmentRepository.UpdateAsync(equipment);
                await unitOfWork.SaveChangesAsync();
                return result;
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new ConcurrencyConflictException("Sprzęt został zmodyfikowany przez innego użytkownika. Odśwież dane i spróbuj ponownie.");
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id nie może być pustym GUID-em.", nameof(id));


            var equipment = await equipmentRepository.GetByIDAsync(id);
            if (equipment == null)
                return;

            if (equipment.Reservations != null && equipment.Reservations.Where(s => s.ReservationStatus == ReservationStatus.Approved).Any())
                throw new IncorrectDataException("Nie można usunąć sprzętu, który ma rezerwacje podrzędne.");

            try
            {
                await equipmentRepository.DeleteAsync(id);
                await unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new ConcurrencyConflictException("Sprzęt jest modyfikowana przez innego użytkownika. Odśwież dane i spróbuj ponownie.");
            }
        }



        public async Task<IEnumerable<Equipment>> GetByCategoryAsync(Guid categoryId)
        {
            if (categoryId == Guid.Empty)
                throw new ArgumentException("Id nie może być pustym GUID-em.", nameof(categoryId));

            return await equipmentRepository.GetByCategoryAsync(categoryId);
        }



        public async Task<Equipment> GetFullEquipmentAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id nie może być pustym GUID-em.", nameof(id));

            return await equipmentRepository.GetFullEquipmentAsync(id);
        }

        public async Task<IEnumerable<Equipment>> GetPagedAsync(int pageNumber, int pageSize)
        {
            if (pageSize == 0)
                throw new IncorrectDataException("Nie podano ilości sprzętów do zwrócenia.");

            return await equipmentRepository.GetPagedAsync(pageNumber, pageSize);
        }

        public async Task<IEnumerable<Equipment>> SearchAsync(string phrase)
        {
            if (string.IsNullOrEmpty(phrase))
                throw new ArgumentNullException(nameof(phrase));

            return await equipmentRepository.SearchAsync(phrase);
        }
    }
}
