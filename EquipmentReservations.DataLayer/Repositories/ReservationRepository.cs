using EquipmentReservations.DataLayer.Dbos;
using EquipmentReservations.Models;
using Microsoft.EntityFrameworkCore;

namespace EquipmentReservations.DataLayer.Repositories
{
    public class ReservationRepository : Repository<Reservation, ReservationDbo>, IReservationRepository
    {
        private readonly EquipmentReservationsDbContext context;
        private readonly IDboConverter converter;

        public ReservationRepository(EquipmentReservationsDbContext context, IDboConverter converter)
            : base(context, converter)
        {
            this.context = context;
            this.converter = converter;
        }

        public async Task<IEnumerable<Reservation>> GetActiveAsync()
        {
            var now = DateTime.Now;
            var dbos = await context.Reservations
                .Where(r => r.Start <= now && (r.End == null || r.End >= now))
                .ToListAsync();

            return dbos.Select(converter.Convert<Reservation>);
        }

        public async Task<IEnumerable<Reservation>> GetByUserAsync(string userId)
        {
            var dbos = await context.Reservations
                .Where(r => r.UserId == userId)
                .AsNoTracking()
                .ToListAsync();

            return dbos.Select(converter.Convert<Reservation>);
        }

        public async Task<bool> IsEquipmentAvailableAsync(Guid equipmentId, DateTime start, DateTime end)
        {
            return !await context.Reservations.AnyAsync(r =>
            r.EquipmentId == equipmentId &&
            r.Start < end &&
            (r.End == null || r.End > start));
        }

        public async Task<IEnumerable<Reservation>> SearchAsync(string phrase)
        {
            var dbos = await context.Reservations
                .Include(r => r.Equipment)
                .Where(r => r.Equipment.Name.Contains(phrase))
                .AsNoTracking()
                .ToListAsync();

            return dbos.Select(converter.Convert<Reservation>);
        }
    }
}
