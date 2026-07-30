using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace MiSalonBellezaNicteHa.Controllers
{
    /// <summary>
    /// CONTROLADOR PRINCIPAL Y DE SEGURIDAD (HomeController)
    /// Maneja el inicio de sesión obligatorio, modal de cuentas de Google y bloqueo de acceso.
    /// </summary>
    public class HomeController : Controller
    {
        // -----------------------------------------------------------------------------
        // ACCIÓN: Index (Página principal de Iniciar Sesión / Pantalla de Bloqueo)
        // -----------------------------------------------------------------------------
        public IActionResult Index()
        {
            // Si el usuario ya está autenticado en sesión, pasa al sistema
            if (HttpContext.Session.GetString("UsuarioLogueado") != null)
            {
                return RedirectToAction("Index", "Servicios");
            }

            // Muestra la vista de Iniciar Sesión (Bloqueo de seguridad)
            return View("~/Views/Auth/Login.cshtml");
        }

        // -----------------------------------------------------------------------------
        // ACCIÓN: IniciarSesion (Procesamiento del Login estándar)
        // -----------------------------------------------------------------------------
        [HttpPost]
        public IActionResult IniciarSesion(string correo, string password)
        {
            if (!string.IsNullOrEmpty(correo) && !string.IsNullOrEmpty(password))
            {
                // Inicia sesión y desbloquea el sistema
                HttpContext.Session.SetString("UsuarioLogueado", correo);

                if (correo == "admin@nicteha.com" || correo == "NICTEha@gamil.com")
                {
                    TempData["SuccessMessage"] = $"¡Bienvenido Administrador ({correo})!";
                    return RedirectToAction("Index", "Admin");
                }

                TempData["SuccessMessage"] = $"¡Sesión iniciada correctamente como {correo}!";
                return RedirectToAction("Index", "Servicios");
            }
            
            ViewData["ErrorMessage"] = "Por favor ingresa un correo y contraseña válidos.";
            return View("~/Views/Auth/Login.cshtml");
        }

        // -----------------------------------------------------------------------------
        // ACCIÓN: IniciarSesionGoogleAccount (Selección de cuenta de Google Modal)
        // -----------------------------------------------------------------------------
        public IActionResult IniciarSesionGoogleAccount(string correo)
        {
            string emailFinal = string.IsNullOrEmpty(correo) ? "NICTEha@gamil.com" : correo;
            
            // Establece la sesión activa con la cuenta de Google seleccionada
            HttpContext.Session.SetString("UsuarioLogueado", emailFinal);
            TempData["SuccessMessage"] = $"¡Conectado exitosamente con tu cuenta de Google ({emailFinal})!";

            if (emailFinal.ToLower().Contains("admin") || emailFinal == "NICTEha@gamil.com")
            {
                return RedirectToAction("Index", "Admin");
            }

            return RedirectToAction("Index", "Servicios");
        }

        // -----------------------------------------------------------------------------
        // ACCIÓN: CerrarSesion (Bloquea el sistema de nuevo)
        // -----------------------------------------------------------------------------
        public IActionResult CerrarSesion()
        {
            HttpContext.Session.Clear();
            TempData["SuccessMessage"] = "Has cerrado sesión correctamente. El acceso al sistema ha sido bloqueado.";
            return RedirectToAction("Index");
        }

        // -----------------------------------------------------------------------------
        // ACCIÓN: CrearCuenta (Vista para registrar un nuevo usuario)
        // -----------------------------------------------------------------------------
        public IActionResult CrearCuenta()
        {
            return View("~/Views/Auth/CrearCuenta.cshtml");
        }

        // -----------------------------------------------------------------------------
        // ACCIÓN: RegistrarUsuario (Procesa el registro de la nueva cuenta)
        // -----------------------------------------------------------------------------
        [HttpPost]
        public IActionResult RegistrarUsuario(string nombre, string correo, string password, string confirmarPassword)
        {
            if (password != confirmarPassword)
            {
                ViewData["ErrorMessage"] = "Las contraseñas no coinciden. Verifícalas e intenta de nuevo.";
                return View("~/Views/Auth/CrearCuenta.cshtml");
            }

            TempData["SuccessMessage"] = $"¡Cuenta creada con éxito para {nombre}! Ya puedes iniciar sesión.";
            return RedirectToAction("Index");
        }
    }
}