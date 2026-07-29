using Microsoft.AspNetCore.Mvc;

namespace MiSalonBellezaNicteHa.Controllers
{
    /// <summary>
    /// Controlador responsable de gestionar la vista de localización geográfica del salón en Mérida, Yucatán.
    /// </summary>
    public class UbicacionController : Controller
    {
        // -----------------------------------------------------------------------------
        // ACCIÓN: Index (Carga la página de ubicación con Google Maps)
        // -----------------------------------------------------------------------------
        public IActionResult Index()
        {
            // Retorna la vista Views/Ubicacion/Index.cshtml con el mapa de Google Maps integrado
            return View();
        }
    }
}
