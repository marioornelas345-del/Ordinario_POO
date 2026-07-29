using Microsoft.AspNetCore.Mvc;

namespace MiSalonBellezaNicteHa.Controllers
{
    /// <summary>
    /// CONTROLADOR DE SEGURIDAD Y AUTENTICACIÓN (AuthController)
    /// Maneja el control de inicio de sesión para los dos perfiles: Administrador y Usuario.
    /// </summary>
    public class AuthController : Controller
    {
        // -----------------------------------------------------------------------------
        // ACCIÓN: Login (Muestra la vista de acceso)
        // -----------------------------------------------------------------------------
        public IActionResult Login()
        {
            return View("~/Views/Auth/Login.cshtml");
        }

        // -----------------------------------------------------------------------------
        // ACCIÓN: Login (Procesa la entrada según el perfil de usuario o admin)
        // -----------------------------------------------------------------------------
        [HttpPost]
        public IActionResult Login(string correo, string password)
        {
            // Perfil Administrador
            if (correo == "admin@nicteha.com" && password == "1234")
            {
                return RedirectToAction("Index", "Admin");
            }

            // Perfil Usuario Estándar
            return RedirectToAction("Index", "Servicios");
        }

        // -----------------------------------------------------------------------------
        // ACCIÓN: Registrar (Muestra el formulario de registro)
        // -----------------------------------------------------------------------------
        public IActionResult Registrar()
        {
            return View("~/Views/Auth/CrearCuenta.cshtml");
        }
    }
}
