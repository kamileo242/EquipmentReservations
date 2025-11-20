using EquipmentReservations.DataLayer.Dbos;
using EquipmentReservations.Models;
using Microsoft.EntityFrameworkCore;

namespace EquipmentReservations.DataLayer.Repositories
{
    public class CategoryRepository : Repository<Category, CategoryDbo>, ICategoryRepository
    {
        private readonly EquipmentReservationsDbContext context;
        private readonly IDboConverter converter;
        public CategoryRepository(EquipmentReservationsDbContext context, IDboConverter converter)
            : base(context, converter)
        {
            this.context = context;
            this.converter = converter;
        }

        public async Task<Category?> GetCategoryTreeAsync(Guid categoryId)
        {
            var dbo = await context.Categories
                .Include(c => c.SubCategories)
                .FirstOrDefaultAsync(c => c.Id == categoryId);

            return dbo == null ? null : converter.Convert<Category>(dbo);
        }

        public async Task<IEnumerable<Category>> GetRootCategoriesAsync()
        {
            var dbos = await context.Categories
                .Where(c => c.ParentCategoryId == null)
                .AsNoTracking()
                .ToListAsync();

            return dbos.Select(converter.Convert<Category>);
        }

        public async Task<IEnumerable<Category>> SearchAsync(string phrase)
        {
            var dbos = await context.Categories
                .Where(c => c.Name.Contains(phrase))
                .ToListAsync();

            return dbos.Select(converter.Convert<Category>);
        }
    }
}
