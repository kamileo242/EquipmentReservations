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
    [Route("api/reservation")]
    [ApiController]
    [Authorize]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Błąd po stronie użytkownika, błędne dane wejściowe do usługi.")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Brak wyszukiwanej rezerwacji w bazie danych.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Błąd wewnętrzny po stronie serwera, np. niespójność danych.")]
    public class ReservationController : ControllerBase
    {
        public const string GetReservationByIdOperationType = "Pobranie rezerwacji po identyfikatorze.";
        public const string GetAllReservationsOperationType = "Pobranie wszystkich rezerwacji.";
        public const string PostReservationOperationType = "Dodanie nowej rezerwacji.";
        public const string PutReservationOperationType = "Aktualizacja istniejącej rezerwacji.";
        public const string GetUserReservationsOperationType = "Pobranie wszystkich rezerwacji użytkownika.";
        public const string GetActiveReservationsOperationType = "Pobranie aktywnych rezerwacji.";
        public const string CheckAvailabilityOperationType = "Sprawdzenie dostępności sprzętu w danym czasie.";
        public const string SearchReservationOperationType = "Wyszukiwanie rezerwacji po nazwie sprzętu.";

        private readonly IReservationService reservationService;
        private readonly IDtoConverter dtoConverter;

        public ReservationController(IReservationService reservationService, IDtoConverter dtoConverter)
        {
            this.reservationService = reservationService;
            this.dtoConverter = dtoConverter;
        }

        /// <summary>
        /// Pobiera rezerwację po identyfikatorze.
        /// </summary>
        /// <param name="id">Identyfikator rezerwacji.</param>
        [HttpGet("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ReservationDto), StatusCodes.Status200OK)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ReservationDtoExample))]
        [SwaggerOperation(GetReservationByIdOperationType)]
        public async Task<IActionResult> GetById([Required] string id)
        {
            var reservation = await reservationService.GetByIdAsync(id.TextToGuid());

            var result = dtoConverter.Convert(reservation);

            return Ok(result);
        }

        /// <summary>
        /// Pobiera wszystkie rezerwacje.
        /// </summary>
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<ReservationDto>), StatusCodes.Status200OK)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ReservationDtoListExample))]
        [SwaggerOperation(GetAllReservationsOperationType)]
        public async Task<IActionResult> GetAll()
        {
            var reservations = await reservationService.GetAllAsync();

            var result = reservations.Select(dtoConverter.Convert);

            return Ok(result);
        }

        /// <summary>
        /// Dodaje nową rezerwację.
        /// </summary>
        /// <param name="dto">Dane rezerwacji do utworzenia.</param>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ReservationDto), StatusCodes.Status201Created)]
        [SwaggerRequestExample(typeof(ReservationStoreDto), typeof(ReservationStoreDtoExample))]
        [SwaggerResponseExample(StatusCodes.Status201Created, typeof(ReservationDtoExample))]
        [SwaggerOperation(PostReservationOperationType)]
        public async Task<IActionResult> Post([FromBody, Required] ReservationStoreDto dto)
        {
            var reservation = dtoConverter.Convert(dto);

            var result = await reservationService.AddAsync(reservation);
            var resultDto = dtoConverter.Convert(result);

            return Ok(resultDto);
        }

        /// <summary>
        /// Aktualizuje istniejącą rezerwację.
        /// </summary>
        /// <param name="id">Identyfikator rezerwacji.</param>
        /// <param name="dto">Zaktualizowane dane rezerwacji.</param>
        [HttpPut("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(ReservationDto), StatusCodes.Status200OK)]
        [SwaggerRequestExample(typeof(ReservationStoreDto), typeof(ReservationStoreDtoExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ReservationDtoExample))]
        [SwaggerOperation(PutReservationOperationType)]
        public async Task<IActionResult> Put([Required] string id, [FromBody, Required] ReservationStoreDto dto)
        {
            var reservation = dtoConverter.Convert(dto);
            reservation.Id = id.TextToGuid();

            var result = await reservationService.UpdateAsync(reservation);
            var resultDto = dtoConverter.Convert(result);

            return Ok(resultDto);
        }

        /// <summary>
        /// Pobiera wszystkie rezerwacje konkretnego użytkownika.
        /// </summary>
        /// <param name="userId">Identyfikator użytkownika.</param>
        [HttpGet("user/{userId}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<ReservationDto>), StatusCodes.Status200OK)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ReservationDtoListExample))]
        [SwaggerOperation(GetUserReservationsOperationType)]
        public async Task<IActionResult> GetByUser([Required] string userId)
        {
            var reservations = await reservationService.GetByUserAsync(userId);

            var result = reservations.Select(dtoConverter.Convert);

            return Ok(result);
        }

        /// <summary>
        /// Sprawdza dostępność sprzętu w określonym przedziale czasu.
        /// </summary>
        /// <param name="equipmentId">Identyfikator sprzętu.</param>
        /// <param name="start">Data rozpoczęcia rezerwacji.</param>
        /// <param name="end">Data zakończenia rezerwacji.</param>
        [HttpGet("availability")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [SwaggerOperation(CheckAvailabilityOperationType)]
        public async Task<IActionResult> CheckAvailability(
            [Required] string equipmentId,
            [Required] DateTime start,
            [Required] DateTime end)
        {
            var available = await reservationService.IsEquipmentAvailableAsync(equipmentId.TextToGuid(), start, end);

            return Ok(available);
        }

        /// <summary>
        /// Pobiera wszystkie aktywne rezerwacje.
        /// </summary>
        [HttpGet("active")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<ReservationDto>), StatusCodes.Status200OK)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ReservationDtoListExample))]
        [SwaggerOperation(GetActiveReservationsOperationType)]
        public async Task<IActionResult> GetActive()
        {
            var reservations = await reservationService.GetActiveAsync();

            var result = reservations.Select(dtoConverter.Convert);

            return Ok(result);
        }

        [HttpGet("browse")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(IEnumerable<ReservationWithEquipmentDto>), StatusCodes.Status200OK)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ReservationWithEquipmentDtoExample))]
        [SwaggerOperation(SearchReservationOperationType)]
        public async Task<IActionResult> Search([FromQuery, Required] string phrase)
        {
            var reservations = await reservationService.SearchAsync(phrase);

            var result = reservations.Select(dtoConverter.ConvertWithEquipment);

            return Ok(result);
        }
    }
}
