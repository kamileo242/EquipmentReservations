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
    [Route("api/audit")]
    [ApiController]
    [Authorize]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Błąd po stronie użytkownika, np. błędne dane wejściowe.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Nie znaleziono rekordu.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Błąd wewnętrzny serwera.")]
    public class AuditLogController : ControllerBase
    {
        public const string GetAuditLogByIdOperationType = "Pobranie logu audytowego po identyfikatorze.";
        public const string GetAllAuditLogsOperationType = "Pobranie wszystkich logów audytowych.";
        public const string GetLatestAuditLogsOperationType = "Pobranie najnowszych logów audytowych.";
        public const string GetAuditLogsForEntityOperationType = "Pobranie logów dla konkretnej encji.";
        public const string SearchAuditLogsOperationType = "Wyszukiwanie logów po użytkowniku.";

        private readonly IAuditLogService auditLogService;
        private readonly IDtoConverter dtoConverter;

        public AuditLogController(IAuditLogService auditLogService, IDtoConverter dtoConverter)
        {
            this.auditLogService = auditLogService;
            this.dtoConverter = dtoConverter;
        }

        /// <summary>
        /// Pobiera log audytowy po identyfikatorze.
        /// </summary>
        [HttpGet("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(AuditLogDto), StatusCodes.Status200OK)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(AuditLogDtoExample))]
        [SwaggerOperation(GetAuditLogByIdOperationType)]
        public async Task<IActionResult> GetById([Required] string id)
        {
            var log = await auditLogService.GetByIdAsync(id.TextToGuid());

            var result = dtoConverter.Convert(log);

            return Ok(result);
        }

        /// <summary>
        /// Pobiera wszystkie logi audytowe.
        /// </summary>
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<AuditLogDto>), StatusCodes.Status200OK)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(AuditLogDtoListExample))]
        [SwaggerOperation(GetAllAuditLogsOperationType)]
        public async Task<IActionResult> GetAll()
        {
            var logs = await auditLogService.GetAllAsync();

            var result = logs.Select(dtoConverter.Convert);

            return Ok(result);
        }

        /// <summary>
        /// Pobiera najnowsze logi audytowe.
        /// </summary>
        /// <param name="count">Ilość najnowszych logów (domyślnie 50)</param>
        [HttpGet("latest")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<AuditLogDto>), StatusCodes.Status200OK)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(AuditLogDtoListExample))]
        [SwaggerOperation(GetLatestAuditLogsOperationType)]
        public async Task<IActionResult> GetLatest([FromQuery] int count = 50)
        {
            var logs = await auditLogService.GetLatestAsync(count);

            var result = logs.Select(dtoConverter.Convert);

            return Ok(result);
        }

        /// <summary>
        /// Pobiera logi dla konkretnej encji.
        /// </summary>
        [HttpGet("entity")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<AuditLogDto>), StatusCodes.Status200OK)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(AuditLogDtoListExample))]
        [SwaggerOperation(GetAuditLogsForEntityOperationType)]
        public async Task<IActionResult> GetForEntity([FromQuery, Required] string entityName, [FromQuery, Required] string entityId)
        {
            var logs = await auditLogService.GetForEntityAsync(entityName, entityId);

            var result = logs.Select(dtoConverter.Convert);

            return Ok(result);
        }

        /// <summary>
        /// Wyszukuje logi audytowe po użytkowniku.
        /// </summary>
        [HttpGet("search")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<AuditLogDto>), StatusCodes.Status200OK)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(AuditLogDtoListExample))]
        [SwaggerOperation(SearchAuditLogsOperationType)]
        public async Task<IActionResult> Search([FromQuery, Required] string changedBy)
        {
            var logs = await auditLogService.SearchAsync(changedBy);

            var result = logs.Select(dtoConverter.Convert);

            return Ok(result);
        }
    }
}
