using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System;

namespace MiSalonBellezaNicteHa.Controllers
{
    /// <summary>
    /// Controlador responsable del proceso de Pago por Transferencia Bancaria,
    /// desglose de montos solicitados, subida de imagen de evidencia y confirmación del pago.
    /// </summary>
    public class PagosController : Controller
    {
        // -----------------------------------------------------------------------------
        // Estado temporal guardado en memoria para conservar los datos de la transferencia
        // -----------------------------------------------------------------------------
        private static string _servicioSolicitado = "Corte Clásico de Hombre";
        private static decimal _montoTotal = 120.00m;
        private static string _evidenciaUrl = string.Empty; // URL de la foto de la transferencia subida
        private static string _estadoPago = "Pendiente de Pago";

        // -----------------------------------------------------------------------------
        // ACCIÓN: Index (Muestra la pantalla con la cantidad de precio, desglose, datos bancarios y evidencia)
        // -----------------------------------------------------------------------------
        public IActionResult Index(int citaId = 0, decimal monto = 0, string servicio = null)
        {
            if (HttpContext.Session.GetString("UsuarioLogueado") == null)
            {
                return RedirectToAction("Index", "Home");
            }
            // Si venimos del flujo de Citas con datos nuevos, actualizamos la orden de pago
            if (monto > 0)
            {
                _montoTotal = monto;
                _estadoPago = "Pendiente de Pago";
                _evidenciaUrl = string.Empty; // Reiniciamos la evidencia para la nueva cita
            }
            
            if (!string.IsNullOrEmpty(servicio))
            {
                _servicioSolicitado = servicio;
            }

            ViewData["CitaId"] = citaId > 0 ? citaId : 101;
            ViewData["Servicio"] = _servicioSolicitado;
            ViewData["MontoTotal"] = _montoTotal;
            ViewData["EvidenciaUrl"] = _evidenciaUrl;
            ViewData["EstadoPago"] = _estadoPago;

            return View();
        }

        // -----------------------------------------------------------------------------
        // ACCIÓN: ConfirmarPago (Recibe la imagen del comprobante de transferencia y confirma el pago)
        // -----------------------------------------------------------------------------
        [HttpPost]
        public IActionResult ConfirmarPago(IFormFile evidenciaPago)
        {
            // 1. Verificación y subida de la imagen del comprobante / evidencia de pago
            if (evidenciaPago != null && evidenciaPago.Length > 0)
            {
                // Ruta destino donde se guardan las evidencias: wwwroot/uploads/
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Generación de un nombre único para el archivo del comprobante
                string uniqueFileName = "comprobante_" + Guid.NewGuid().ToString().Substring(0, 8) + Path.GetExtension(evidenciaPago.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Guardamos el comprobante físicamente en el servidor
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    evidenciaPago.CopyTo(fileStream);
                }

                // Actualizamos la URL relativa de la evidencia
                _evidenciaUrl = "/uploads/" + uniqueFileName;
                _estadoPago = "Completado / Pago Confirmado";
                
                TempData["SuccessMessage"] = "¡Comprobante de pago recibido correctamente! Tu transferencia ha sido validada con éxito.";
            }
            else
            {
                TempData["ErrorMessage"] = "Por favor selecciona una imagen del comprobante de tu transferencia antes de confirmar.";
            }

            return RedirectToAction("Index");
        }
    }
}
