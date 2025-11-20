using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using EquipmentReservations.Domain;
using EquipmentReservations.WebApi.Converters;
using EquipmentReservations.WebApi.Dtos;
using EquipmentReservations.WebApi.Examples;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace EquipmentReservations.WebApi.Controllers
{
    [Route("api/category")]
    [ApiController]
    [Authorize]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Błąd po stronie użytkownika, błędne dane wejściowe do usługi.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Brak wyszukiwanej kategorii w bazie danych.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Błąd wewnętrzny po stronie serwera, np. niespójność danych.")]
    public class CategoryController : ControllerBase
    {
        public const string GetCategoryByIdOperationType = "Pobranie kategorii po identyfikatorze.";
        public const string GetAllCategoriesOperationType = "Pobranie wszystkich kategori.";
        public const string PostCategoryOperationType = "Dodanie kategorii.";
        public const string PutCategoryOperationType = "Zmodyfikowanie kategorii.";
        public const string DeleteCategoryOperationType = "Usunięcie kategorii.";
        public const string GetRootCategoriesOperationType = "Pobranie wszystkich głównych kategorii.";
        public const string GetCategoryTreeOperationType = "Pobranie drzewa kategorii.";
        public const string BrowseCategoryOperationType = "Pobranie kategorii po wyszukiwanej frazie.";

        private readonly ICategoryService categoryService;
        private readonly IDtoConverter dtoConverter;

        public CategoryController(ICategoryService categoryService, IDtoConverter dtoConverter)
        {
            this.categoryService = categoryService;
            this.dtoConverter = dtoConverter;
        }

        /// <summary>
        /// Pobiera kategorie po identyfikatorze
        /// </summary>
        /// <param name="id">Identyfikator kategorii</param>
        /// <returns>Kategoria o podanym identyfikatorze</returns>
        [HttpGet("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Nie znaleziono kategorii.")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(CategoryDtoExample))]
        [SwaggerOperation(GetCategoryByIdOperationType)]
        public async Task<IActionResult> GetById([Required] string id)
        {
            var category = await categoryService.GetByIdAsync(id.TextToGuid());

            var result = dtoConverter.Convert(category);

            return Ok(result);
        }

        /// <summary>
        /// Pobiera wszystkie kategorie.
        /// </summary>
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<CategoryDto>), StatusCodes.Status200OK)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(CategoryDtoListExample))]
        [SwaggerOperation(GetAllCategoriesOperationType)]
        public async Task<IActionResult> GetAll()
        {
            var categories = await categoryService.GetAllAsync();

            var result = categories.Select(dtoConverter.Convert);

            return Ok(result);
        }

        /// <summary>
        /// Dodaje nową kategorię.
        /// </summary>
        /// <param name="dto">Dane kategorii do dodania.</param>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status201Created)]
        [SwaggerResponseExample(StatusCodes.Status201Created, typeof(CategoryDtoExample))]
        [SwaggerRequestExample(typeof(StoreCategoryDto), typeof(StoreCategoryDtoExample))]
        [SwaggerOperation(PostCategoryOperationType)]
        public async Task<IActionResult> Post([FromBody, Required] StoreCategoryDto dto)
        {
            var category = dtoConverter.Convert(dto);

            var result = await categoryService.AddAsync(category);

            var resultDto = dtoConverter.Convert(result);

            return Ok(resultDto);
        }

        /// <summary>
        /// Aktualizuje istniejącą kategorię.
        /// </summary>
        /// <param name="id">Identyfikator kategorii.</param>
        /// <param name="dto">Dane kategorii do aktualizacji.</param>
        [HttpPut("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(CategoryDtoExample))]
        [SwaggerRequestExample(typeof(StoreCategoryDto), typeof(StoreCategoryDtoExample))]
        [SwaggerOperation(PutCategoryOperationType)]
        public async Task<IActionResult> Put([Required] string id, [FromBody] StoreCategoryDto dto)
        {
            var category = dtoConverter.Convert(dto);
            category.Id = id.TextToGuid();

            var result = await categoryService.UpdateAsync(category);

            var resultDto = dtoConverter.Convert(result);

            return Ok(resultDto);
        }

        /// <summary>
        /// Usuwa kategorię o podanym identyfikatorze.
        /// </summary>
        /// <param name="id">Identyfikator kategorii.</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [SwaggerOperation(DeleteCategoryOperationType)]
        public async Task<IActionResult> Delete([Required] string id)
        {
            await categoryService.DeleteAsync(id.TextToGuid());

            return NoContent();
        }

        /// <summary>
        /// Pobiera wszystkie główne (root) kategorie.
        /// </summary>
        [HttpGet("root")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<CategoryDto>), StatusCodes.Status200OK)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(CategoryDtoListExample))]
        [SwaggerOperation(GetRootCategoriesOperationType)]
        public async Task<IActionResult> GetRootCategories()
        {
            var roots = await categoryService.GetRootCategoriesAsync();

            var result = roots.Select(dtoConverter.Convert);

            return Ok(result);
        }

        /// <summary>
        /// Pobiera całe drzewo kategorii.
        /// </summary>
        [HttpGet("tree")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<CategoryTreeDto>), StatusCodes.Status200OK)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(CategoryTreeDtoExample))]
        [SwaggerOperation(GetCategoryTreeOperationType)]
        public async Task<IActionResult> GetTree([Required] string id)
        {
            var tree = await categoryService.GetCategoryTreeAsync(id.TextToGuid());

            var result = dtoConverter.ConvertTree(tree);

            return Ok(result);
        }

        /// <summary>
        /// Wyszukuje kategorie po frazie tekstowej.
        /// </summary>
        /// <param name="query">Fraza wyszukiwania.</param>
        [HttpGet("browse")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<CategoryDto>), StatusCodes.Status200OK)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(CategoryDtoListExample))]
        [SwaggerOperation(BrowseCategoryOperationType)]
        public async Task<IActionResult> Browse([FromQuery, Required] string query)
        {
            var categories = await categoryService.SearchAsync(query);

            var result = categories.Select(dtoConverter.Convert);

            return Ok(result);
        }
    }
}
