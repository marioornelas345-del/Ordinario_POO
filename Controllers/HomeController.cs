using Microsoft.AspNetCore.Mvc;

namespace MiSalonBellezaNicteHa.Controllers
{
    /// <summary>
    /// Controlador principal para el manejo de la autenticación (Login, Registro y Google OAuth).
    /// </summary>
    public class HomeController : Controller
    {
        // -----------------------------------------------------------------------------
        // ACCIÓN: Index (Página principal de Iniciar Sesión)
        // -----------------------------------------------------------------------------
        public IActionResult Index()
        {
            // Muestra la vista de Iniciar Sesión (Login)
            return View("~/Views/Auth/Login.cshtml");
        }

        // -----------------------------------------------------------------------------
        // ACCIÓN: IniciarSesion (Procesamiento del Login estándar)
        // -----------------------------------------------------------------------------
        [HttpPost]
        public IActionResult IniciarSesion(string correo, string password)
        {
            // Validación simulada de credenciales (Admin vs Usuario)
            if (correo == "admin@nicteha.com" && password == "1234")
            {
                // Si las credenciales son válidas, redirigimos al catálogo de servicios
                return RedirectToAction("Index", "Servicios");
            }
            
            // Mensaje de error si la contraseña o correo no coinciden
            ViewData["ErrorMessage"] = "Correo o contraseña incorrectos. Intenta nuevamente.";
            return View("~/Views/Auth/Login.cshtml");
        }

        // -----------------------------------------------------------------------------
        // ACCIÓN: CrearCuenta (Vista para registrar un nuevo usuario)
        // -----------------------------------------------------------------------------
        public IActionResult CrearCuenta()
        {
            // Muestra el formulario para crear una nueva cuenta en el sistema
            return View("~/Views/Auth/CrearCuenta.cshtml");
        }

        // -----------------------------------------------------------------------------
        // ACCIÓN: RegistrarUsuario (Procesa el registro de la nueva cuenta)
        // -----------------------------------------------------------------------------
        [HttpPost]
        public IActionResult RegistrarUsuario(string nombre, string correo, string password, string confirmarPassword)
        {
            // Verificación de contraseñas coincidentes
            if (password != confirmarPassword)
            {
                ViewData["ErrorMessage"] = "Las contraseñas no coinciden. Verifícalas e intenta de nuevo.";
                return View("~/Views/Auth/CrearCuenta.cshtml");
            }

            // Mensaje de éxito tras el registro
            TempData["SuccessMessage"] = $"¡Cuenta creada con éxito para {nombre}! Ya puedes iniciar sesión.";
            return RedirectToAction("Index");
        }

        // -----------------------------------------------------------------------------
        // ACCIÓN: IniciarSesionGoogle (Simula el inicio de sesión / emparejamiento con Google)
        // -----------------------------------------------------------------------------
        public IActionResult IniciarSesionGoogle()
        {
            // En una aplicación real, aquí se usaría Microsoft.AspNetCore.Authentication.Google
            // Simulamos el emparejamiento con Google mandando al usuario logueado a Servicios.
            TempData["SuccessMessage"] = "¡Sesión iniciada correctamente con tu cuenta de Google!";
            return RedirectToAction("Index", "Servicios");
        }
    }
}