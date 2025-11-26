using EquipmentReservations.DataLayer;
using EquipmentReservations.Models;
using EquipmentReservations.Models.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace EquipmentReservations.Domain.Services
{
  public class CategoryService : ICategoryService
  {
    private readonly ICategoryRepository categoryRepository;
    private readonly IUnitOfWork unitOfWork;
    private readonly ICacheService cacheService;
    private readonly IGuidProvider guidProvider;

    private const string AllCategoriesCacheKey = "categories_all";
    private const string RootCategoriesCacheKey = "categories_root";
    private int cacheRestartTime = 30;

    public CategoryService(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork,
      ICacheService cacheService, IGuidProvider guidProvider)
    {
      this.categoryRepository = categoryRepository;
      this.unitOfWork = unitOfWork;
      this.cacheService = cacheService;
      this.guidProvider = guidProvider;
    }
    public async Task<Category> GetByIdAsync(Guid id)
    {
      if (id == Guid.Empty)
        throw new ArgumentException("Id nie może być pustym GUID-em.", nameof(id));

      string cacheKey = $"category_{id}";
      var cached = cacheService.Get<Category>(cacheKey);

      if (cached != null)
        return cached;

      var category = await categoryRepository.GetByIDAsync(id);
      if (category == null)
        throw new MissingDataException($"Nie znaleziono kategorii o id: {id}");

      cacheService.Set(cacheKey, category, TimeSpan.FromMinutes(cacheRestartTime));
      return category;
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
      var cached = cacheService.Get<IEnumerable<Category>>(AllCategoriesCacheKey);
      if (cached != null)
        return cached;

      var categories = await categoryRepository.GetAllAsync();
      cacheService.Set(AllCategoriesCacheKey, categories, TimeSpan.FromMinutes(cacheRestartTime));
      return categories;
    }

    public async Task<Category> AddAsync(Category category)
    {
      if (category == null)
        throw new ArgumentNullException(nameof(category));

      if (string.IsNullOrEmpty(category.Name))
        throw new IncorrectDataException("Nazwa kategorii jest wymagana.");

      if (category.ParentCategoryId != null)
      {
        var parent = await categoryRepository.GetByIDAsync(category.ParentCategoryId.Value);
        if (parent == null)
          throw new MissingDataException($"Nie znaleziono kategorii nadrzędnej o id: {category.ParentCategoryId}");

        category.ParentCategory = parent;
      }

      category.Id = guidProvider.GenerateGuid();
      var added = await categoryRepository.AddAsync(category);
      await unitOfWork.SaveChangesAsync();
      var result = await categoryRepository.GetByIDAsync(added.Id);
      ClearCache(result.Id);
      return result;
    }

    public async Task<Category> UpdateAsync(Category category)
    {
      if (category == null)
        throw new ArgumentNullException(nameof(category));

      var existingCategory = await categoryRepository.GetByIDAsync(category.Id);
      if (existingCategory == null)
        throw new MissingDataException($"Nie znaleziono kategorii do modyfikacji o id: {category.Id}");

      if (category.ParentCategoryId != null)
      {
        var parent = await categoryRepository.GetByIDAsync(category.ParentCategoryId.Value);
        if (parent == null)
          throw new MissingDataException($"Nie znaleziono kategorii nadrzędnej o id: {category.ParentCategoryId}");

        if (existingCategory.SubCategories != null && category.SubCategories.Any())
          throw new IncorrectDataException($"Nie można zmieniać kategorii nadrzędnej kategorii, która posiada kategorie podrzędne.");

        category.ParentCategory = parent;
      }
      try
      {
        var updated = await categoryRepository.UpdateAsync(category);
        await unitOfWork.SaveChangesAsync();
        var result = await categoryRepository.GetByIDAsync(updated.Id);
        ClearCache(result.Id);
        return result;
      }
      catch (DbUpdateConcurrencyException)
      {
        throw new ConcurrencyConflictException("Kategoria została zmodyfikowana przez innego użytkownika. Odśwież dane i spróbuj ponownie.");
      }
    }

    public async Task DeleteAsync(Guid id)
    {
      if (id == Guid.Empty)
        throw new ArgumentException("Id nie może być pustym GUID-em.", nameof(id));

      var category = await categoryRepository.GetByIDAsync(id);
      if (category == null)
        return;

      if (category.SubCategories != null && category.SubCategories.Any())
        throw new IncorrectDataException("Nie można usunąć kategorii, która ma kategorie podrzędne.");

      if (category.Equipments != null && category.Equipments.Any())
        throw new IncorrectDataException("Nie można usunąć kategorii, która zawiera sprzęt.");

      try
      {
        await categoryRepository.DeleteAsync(id);
        await unitOfWork.SaveChangesAsync();
        ClearCache(id);
      }
      catch (DbUpdateConcurrencyException)
      {
        throw new ConcurrencyConflictException("Kategoria jest modyfikowana przez innego użytkownika. Odśwież dane i spróbuj ponownie.");
      }
    }

    private void ClearCache(Guid? id = null)
    {
      cacheService.Remove(AllCategoriesCacheKey);
      cacheService.Remove(RootCategoriesCacheKey);

      if (id.HasValue)
        cacheService.Remove($"category_{id.Value}");
    }

    public async Task<IEnumerable<Category>> GetRootCategoriesAsync()
    {
      var cached = cacheService.Get<IEnumerable<Category>>(RootCategoriesCacheKey);
      if (cached != null)
        return cached;

      var rootCategories = await categoryRepository.GetRootCategoriesAsync();
      cacheService.Set(RootCategoriesCacheKey, rootCategories, TimeSpan.FromMinutes(cacheRestartTime));
      return rootCategories;
    }

    public async Task<Category?> GetCategoryTreeAsync(Guid categoryId)
    {
      if (categoryId == Guid.Empty)
        throw new ArgumentException("Id nie może być pustym GUID-em.", nameof(categoryId));

      return await categoryRepository.GetCategoryTreeAsync(categoryId);
    }

    public async Task<IEnumerable<Category>> SearchAsync(string phrase)
    {
      if (string.IsNullOrEmpty(phrase))
        throw new ArgumentNullException(nameof(phrase));

      return await categoryRepository.SearchAsync(phrase);
    }
  }
}
