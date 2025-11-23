using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace EquipmentReservations.DataLayer.Repositories
{
    /// <summary>
    /// Abstrakcyjne repozytorium generyczne dla operacji CRUD
    /// </summary>
    /// <typeparam name="TModel">Model biznesowy</typeparam>
    /// <typeparam name="TDbo">Reprezentacja modelu w bazie danych.</typeparam>
    public abstract class Repository<TModel, TDbo> : IRepository<TModel>
        where TModel : class
        where TDbo : class
    {

        private readonly EquipmentReservationsDbContext context;
        private readonly IDboConverter converter;
        private readonly DbSet<TDbo> dbSet;

        public Repository(EquipmentReservationsDbContext context, IDboConverter converter)
        {
            this.context = context;
            this.converter = converter;
            dbSet = context.Set<TDbo>();
        }
        /// <inheritdoc/>
        public async Task<TModel?> GetByIDAsync(Guid id)
        {
            var dbo = await dbSet.FindAsync(id);
            return dbo == null
                ? null
                : converter.Convert<TModel>(dbo);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TModel>> GetAllAsync()
        {
            var dbos = await dbSet.AsNoTracking().ToListAsync();
            return dbos.Select(dbo => converter.Convert<TModel>(dbo));
        }


        /// <inheritdoc/>
        public async Task<TModel> AddAsync(TModel entity)
        {
            var dbo = converter.Convert<TDbo>(entity);
            await dbSet.AddAsync(dbo);
            return converter.Convert<TModel>(dbo);
        }

        /// <inheritdoc/>
        public async Task<TModel> UpdateAsync(TModel entity)
        {
            var newDbo = converter.Convert<TDbo>(entity);
            var idProp = typeof(TDbo).GetProperty("Id", BindingFlags.Public | BindingFlags.Instance);
            var idValueObj = idProp?.GetValue(newDbo);
            var existing = await dbSet.FindAsync(idValueObj);

            context.Entry(existing).CurrentValues.SetValues(newDbo);
            bool saveFailed;

            return converter.Convert<TModel>(existing);
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(Guid id)
        {
            var dbo = await dbSet.FindAsync(id);
            if (dbo == null)
                return;
            dbSet.Remove(dbo);
        }
    }
}
