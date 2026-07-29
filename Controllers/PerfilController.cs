using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System;

namespace MiSalonBellezaNicteHa.Controllers
{
    /// <summary>
    /// CONTROLADOR DEDICADO AL PERFIL DE CLIENTE / USUARIO
    /// Gestiona de manera independiente la información personal, foto y estado del Cliente.
    /// </summary>
    public class PerfilController : Controller
    {
        private static string _nombreCliente = "Mario Isaac (Cliente)";
        private static string _correoCliente = "NICTEha@gamil.com";
        private static string _telefonoCliente = "9991742323";
        private static string _fotoUrlCliente = "/images/sin-foto.jpg";

        // -----------------------------------------------------------------------------
        // ACCIÓN: Index (Muestra la vista exclusiva del Perfil de Cliente)
        // -----------------------------------------------------------------------------
        public IActionResult Index()
        {
            ViewData["Nombre"] = _nombreCliente;
            ViewData["Correo"] = _correoCliente;
            ViewData["Telefono"] = _telefonoCliente;
            ViewData["FotoUrl"] = _fotoUrlCliente;
            ViewData["Rol"] = "Cliente Registrado";

            return View();
        }

        // -----------------------------------------------------------------------------
        // ACCIÓN: ActualizarPerfil (Procesa los datos y foto de perfil del Cliente)
        // -----------------------------------------------------------------------------
        [HttpPost]
        public IActionResult ActualizarPerfil(string nombre, string correo, string telefono, IFormFile fotoPerfil)
        {
            if (!string.IsNullOrWhiteSpace(nombre)) _nombreCliente = nombre;
            if (!string.IsNullOrWhiteSpace(correo)) _correoCliente = correo;
            if (!string.IsNullOrWhiteSpace(telefono)) _telefonoCliente = telefono;

            if (fotoPerfil != null && fotoPerfil.Length > 0)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string uniqueFileName = "cliente_" + Guid.NewGuid().ToString().Substring(0, 8) + Path.GetExtension(fotoPerfil.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    fotoPerfil.CopyTo(fileStream);
                }

                _fotoUrlCliente = "/uploads/" + uniqueFileName;
            }

            TempData["SuccessMessage"] = "¡Perfil de Cliente actualizado correctamente!";
            return RedirectToAction("Index");
        }
    }
}
