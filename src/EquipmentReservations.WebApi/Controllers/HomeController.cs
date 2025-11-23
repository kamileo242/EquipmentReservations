using Microsoft.AspNetCore.Mvc;

namespace EquipmentReservations.WebApi.Controllers
{
    /// <summary>
    /// Informacje o usłudze.
    /// </summary>
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("")]
    public class HomeController : ControllerBase
    {
        /// <summary>
        /// Zwraca informację o usłudze.
        /// </summary>
        /// <returns>Nazwa, wersja  i opis usługi.</returns>
        [HttpGet, HttpHead]
        public IActionResult Index()
        {
            return Content("EquipmentReservation -  1.0 - Wypożyczanie sprzętów.");
        }
    }
}
