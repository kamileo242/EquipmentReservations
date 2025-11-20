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
    [Route("api/equipment")]
    [ApiController]
    [Authorize]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Błąd po stronie użytkownika, błędne dane wejściowe do usługi.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Nie znaleziono sprzętu.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Błąd wewnętrzny po stronie serwera.")]
    public class EquipmentController : ControllerBase
    {
        public const string GetEquipmentByIdOperationType = "Pobranie sprzętu po identyfikatorze.";
        public const string GetAllEquipmentOperationType = "Pobranie wszystkich sprzętów.";
        public const string PostEquipmentOperationType = "Dodanie nowego sprzętu.";
        public const string PutEquipmentOperationType = "Aktualizacja sprzętu.";
        public const string DeleteEquipmentOperationType = "Usunięcie sprzętu.";
        public const string GetEquipmentByCategoryOperationType = "Pobranie sprzętu z danej kategorii.";
        public const string SearchEquipmentOperationType = "Wyszukiwanie sprzętu po nazwie lub opisie.";
        public const string GetFullEquipmentOperationType = "Pobranie sprzętu z pełnymi informacjami o kategoriach i rezerwacjach.";
        public const string GetPagedEquipmentOperationType = "Pobranie sprzętu w sposób stronicowany.";

        private readonly IEquipmentService equipmentService;
        private readonly IDtoConverter dtoConverter;

        public EquipmentController(IEquipmentService equipmentService, IDtoConverter dtoConverter)
        {
            this.equipmentService = equipmentService;
            this.dtoConverter = dtoConverter;
        }

        /// <summary>
        /// Pobiera sprzęt po identyfikatorze.
        /// </summary>
        [HttpGet("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(EquipmentDto), StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Nie znaleziono sprzętu.")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(EquipmentDtoExample))]
        [SwaggerOperation(GetEquipmentByIdOperationType)]
        public async Task<IActionResult> GetById([Required] string id)
        {
            var equipment = await equipmentService.GetByIdAsync(id.TextToGuid());

            var result = dtoConverter.Convert(equipment);

            return Ok(result);
        }

        /// <summary>
        /// Pobiera wszystkie sprzęty.
        /// </summary>
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<EquipmentDto>), StatusCodes.Status200OK)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(EquipmentDtoListExample))]
        [SwaggerOperation(GetAllEquipmentOperationType)]
        public async Task<IActionResult> GetAll()
        {
            var equipments = await equipmentService.GetAllAsync();

            var result = equipments.Select(dtoConverter.Convert);

            return Ok(result);
        }

        /// <summary>
        /// Dodaje nowy sprzęt.
        /// </summary>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(EquipmentDto), StatusCodes.Status201Created)]
        [SwaggerRequestExample(typeof(StoreEquipmentDto), typeof(StoreEquipmentDtoExample))]
        [SwaggerResponseExample(StatusCodes.Status201Created, typeof(EquipmentDtoExample))]
        [SwaggerOperation(PostEquipmentOperationType)]
        public async Task<IActionResult> Post([FromBody, Required] StoreEquipmentDto dto)
        {
            var model = dtoConverter.Convert(dto);

            var result = await equipmentService.AddAsync(model);

            var resultDto = dtoConverter.Convert(result);

            return Ok(resultDto);
        }

        /// <summary>
        /// Aktualizuje dane sprzętu.
        /// </summary>
        [HttpPut("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(EquipmentDto), StatusCodes.Status200OK)]
        [SwaggerRequestExample(typeof(StoreEquipmentDto), typeof(StoreEquipmentDtoExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(EquipmentDtoExample))]
        [SwaggerOperation(PutEquipmentOperationType)]
        public async Task<IActionResult> Put([Required] string id, [FromBody] StoreEquipmentDto dto)
        {
            var model = dtoConverter.Convert(dto);
            model.Id = id.TextToGuid();

            var result = await equipmentService.UpdateAsync(model);
            var resultDto = dtoConverter.Convert(result);

            return Ok(resultDto);
        }

        /// <summary>
        /// Usuwa sprzęt po identyfikatorze.
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [SwaggerOperation(DeleteEquipmentOperationType)]
        public async Task<IActionResult> Delete([Required] string id)
        {
            await equipmentService.DeleteAsync(id.TextToGuid());
            return NoContent();
        }

        /// <summary>
        /// Pobiera sprzęt przypisany do danej kategorii.
        /// </summary>
        [HttpGet("category/{categoryId}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<EquipmentDto>), StatusCodes.Status200OK)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(EquipmentDtoListExample))]
        [SwaggerOperation(GetEquipmentByCategoryOperationType)]
        public async Task<IActionResult> GetByCategory([Required] string categoryId)
        {
            var equipments = await equipmentService.GetByCategoryAsync(categoryId.TextToGuid());

            var result = equipments.Select(dtoConverter.Convert);

            return Ok(result);
        }

        /// <summary>
        /// Wyszukuje sprzęt po frazie tekstowej (nazwa lub opis).
        /// </summary>
        [HttpGet("search")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<EquipmentDto>), StatusCodes.Status200OK)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(EquipmentDtoListExample))]
        [SwaggerOperation(SearchEquipmentOperationType)]
        public async Task<IActionResult> Search([FromQuery, Required] string phrase)
        {
            var equipments = await equipmentService.SearchAsync(phrase);

            var result = equipments.Select(dtoConverter.Convert);

            return Ok(result);
        }

        /// <summary>
        /// Pobiera pełne dane sprzętu, w tym informacje o kategoriach i rezerwacjach.
        /// </summary>
        [HttpGet("full/{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(FullEquipmentDtoExample), StatusCodes.Status200OK)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(EquipmentDtoExample))]
        [SwaggerOperation(GetFullEquipmentOperationType)]
        public async Task<IActionResult> GetFull([Required] string id)
        {
            var equipment = await equipmentService.GetFullEquipmentAsync(id.TextToGuid());

            var result = dtoConverter.ConvertFull(equipment);

            return Ok(result);
        }

        /// <summary>
        /// Pobiera sprzęt w sposób stronicowany.
        /// </summary>
        [HttpGet("paged")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<EquipmentDto>), StatusCodes.Status200OK)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(EquipmentDtoListExample))]
        [SwaggerOperation(GetPagedEquipmentOperationType)]
        public async Task<IActionResult> GetPaged([FromQuery, Range(0, int.MaxValue)] int pageNumber = 0, [FromQuery, Range(1, 100)] int pageSize = 10)
        {
            var equipments = await equipmentService.GetPagedAsync(pageNumber, pageSize);

            var result = equipments.Select(dtoConverter.Convert);

            return Ok(result);
        }
    }
}
