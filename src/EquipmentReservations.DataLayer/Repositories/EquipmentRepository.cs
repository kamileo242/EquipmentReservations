using EquipmentReservations.DataLayer.Dbos;
using EquipmentReservations.Models;
using Microsoft.EntityFrameworkCore;

namespace EquipmentReservations.DataLayer.Repositories
{
    public class EquipmentRepository : Repository<Equipment, EquipmentDbo>, IEquipmentRepository
    {
        private readonly EquipmentReservationsDbContext context;
        private readonly IDboConverter converter;
        public EquipmentRepository(EquipmentReservationsDbContext context, IDboConverter converter)
            : base(context, converter)
        {
            this.context = context;
            this.converter = converter;
        }

        public async Task<IEnumerable<Equipment>> GetByCategoryAsync(Guid categoryId)
        {
            var dbos = await context.Equipments
                .Where(e => e.CategoryId == categoryId)
                .AsNoTracking()
                .ToListAsync();

            return dbos.Select(converter.Convert<Equipment>);
        }

        public async Task<Equipment?> GetFullEquipmentAsync(Guid id)
        {
            var dbo = await context.Equipments
                .Include(e => e.Category)
                .Include(e => e.Reservations)
                .FirstOrDefaultAsync(e => e.Id == id);

            return dbo == null ? null : converter.Convert<Equipment>(dbo);
        }

        public async Task<IEnumerable<Equipment>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var dbos = await context.Equipments
                .OrderBy(e => e.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            return dbos.Select(converter.Convert<Equipment>);
        }

        public async Task<IEnumerable<Equipment>> SearchAsync(string phrase)
        {
            var dbos = await context.Equipments
                .Where(e => e.Name.Contains(phrase) || e.Description.Contains(phrase))
                .ToListAsync();

            return dbos.Select(converter.Convert<Equipment>);
        }
    }
}
