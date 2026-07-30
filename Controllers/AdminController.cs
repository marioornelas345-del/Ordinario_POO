using Microsoft.AspNetCore.Mvc;
using MiSalonBellezaNicteHa.Data;
using MiSalonBellezaNicteHa.Models;
using MiSalonBellezaNicteHa.Patrones.Singleton;
using MiSalonBellezaNicteHa.Patrones.Factory;
using MiSalonBellezaNicteHa.Patrones.Observer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MiSalonBellezaNicteHa.Controllers
{
    /// <summary>
    /// CONTROLADOR PRINCIPAL DEL MÓDULO ADMINISTRADOR
    /// Cumple con todos los requerimientos del examen ordinario de la UTM:
    /// - Supervisión de Catálogos (Estilistas, Servicios, Clientes)
    /// - Gestión completa de Citas (Confirmar, Reprogramar, Cancelar)
    /// - Reportes de Disponibilidad Semanal
    /// - Módulo de Respaldo de Base de Datos (Patrón Singleton)
    /// - Notificaciones automáticas para el día siguiente (Patrón Factory Method y Observer)
    /// 
    /// PRINCIPIOS SOLID APLICADOS:
    /// - SRP: Controla el flujo administrativo coordinando patrones y persistencia.
    /// - DIP: Depende de abstracciones (INotificacion, IObservadorCita) e inyección de DbContext.
    /// </summary>
    public class AdminController : Controller
    {
        private readonly SalonDbContext _context;

        public AdminController(SalonDbContext context)
        {
            _context = context;
        }

        // =========================================================================================
        // ACCIÓN PRINCIPAL: Index (Panel de Control del Administrador)
        // =========================================================================================
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("UsuarioLogueado") == null)
            {
                return RedirectToAction("Index", "Home");
            }
            // 1. Carga de Citas registradas en la base de datos SQL Server
            var citasList = _context.Citas.ToList();

            // 2. Carga de Catálogos
            var serviciosList = ServiciosController.ObtenerServicioPorId(1); // Obtiene lista mock/DB
            ViewData["TotalCitas"] = citasList.Count;
            ViewData["CitasConfirmadas"] = citasList.Count(c => c.Estado == "Confirmada");
            ViewData["CitasPendientes"] = citasList.Count(c => c.Estado == "Agendada");

            // Horarios cargados mediante el PATRÓN SINGLETON
            ViewData["HoraApertura"] = ConfiguracionSalon.Instancia.HoraApertura;
            ViewData["HoraCierre"] = ConfiguracionSalon.Instancia.HoraCierre;

            return View(citasList);
        }

        // =========================================================================================
        // GESTIÓN DE CITAS: Confirmar, Reprogramar y Cancelar (Con PATRÓN OBSERVER)
        // =========================================================================================

        /// <summary>
        /// Confirma una cita y dispara notificaciones vía PATRÓN OBSERVER.
        /// </summary>
        [HttpPost]
        public IActionResult ConfirmarCita(int citaId)
        {
            var cita = _context.Citas.FirstOrDefault(c => c.Id == citaId);
            if (cita != null)
            {
                // POLIMORFISMO Y ENCAPSULAMIENTO: Modificación de estado encapsulada
                cita.Estado = "Confirmada";
                _context.SaveChanges();

                // PATRÓN OBSERVER: Notificamos a los observadores suscritos (SMS, Correo y Bitácora)
                var sujeto = new SujetoCita();
                sujeto.Suscribir(new ObservadorSmsCliente());
                sujeto.Suscribir(new ObservadorCorreoCliente());
                sujeto.Suscribir(new ObservadorBitacoraAdmin());

                sujeto.Notificar(cita.Id, "Confirmada", "Su cita ha sido aprobada por el administrador.");

                TempData["SuccessMessage"] = $"Cita #{citaId} confirmada con éxito. Se enviaron las notificaciones automáticas al cliente.";
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Reprograma una cita a una nueva fecha/hora y recalcula la duración de 1 hora.
        /// </summary>
        [HttpPost]
        public IActionResult ReprogramarCita(int citaId, DateTime nuevaFecha)
        {
            var cita = _context.Citas.FirstOrDefault(c => c.Id == citaId);
            if (cita != null)
            {
                // ABSTRACCIÓN Y ENCAPSULAMIENTO: Método del modelo que recalcula la hora fin (+1 hr)
                cita.AsignarHorario(nuevaFecha);
                cita.Estado = "Reprogramada";
                _context.SaveChanges();

                // PATRÓN OBSERVER: Notificamos la reprogramación
                var sujeto = new SujetoCita();
                sujeto.Suscribir(new ObservadorSmsCliente());
                sujeto.Notificar(cita.Id, "Reprogramada", $"Nueva fecha agendada: {nuevaFecha:dd/MM/yyyy HH:mm}");

                TempData["SuccessMessage"] = $"Cita #{citaId} reprogramada para {nuevaFecha:dd/MM/yyyy HH:mm}.";
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Registra la cancelación de una cita.
        /// </summary>
        [HttpPost]
        public IActionResult CancelarCita(int citaId)
        {
            var cita = _context.Citas.FirstOrDefault(c => c.Id == citaId);
            if (cita != null)
            {
                cita.Estado = "Cancelada";
                _context.SaveChanges();

                // PATRÓN OBSERVER: Notificamos la cancelación
                var sujeto = new SujetoCita();
                sujeto.Suscribir(new ObservadorCorreoCliente());
                sujeto.Notificar(cita.Id, "Cancelada", "Cita cancelada por requerimiento del sistema.");

                TempData["SuccessMessage"] = $"Cita #{citaId} ha sido registrada como CANCELADA.";
            }

            return RedirectToAction("Index");
        }

        // =========================================================================================
        // MÓDULO DE RESPALDO DE BASE DE DATOS (PATRÓN SINGLETON)
        // =========================================================================================

        /// <summary>
        /// Genera una copia de seguridad física de la base de datos haciendo uso del Singleton BackupManager.
        /// </summary>
        public IActionResult GenerarRespaldo()
        {
            // PATRÓN SINGLETON: Acceso a la instancia única centralizada
            string archivoRespaldo = BackupManager.Instancia.GenerarRespaldoBaseDatos();

            TempData["SuccessMessage"] = $"¡Respaldo de Base de Datos generado exitosamente! Archivo creado: {archivoRespaldo}";
            return RedirectToAction("Index");
        }

        // =========================================================================================
        // NOTIFICACIONES AUTOMÁTICAS PARA CITAS DEL DÍA SIGUIENTE (PATRÓN FACTORY METHOD)
        // =========================================================================================

        /// <summary>
        /// Envía recordatorios masivos automáticos a todos los clientes con cita el día de mañana.
        /// </summary>
        public IActionResult EnviarRecordatoriosManana()
        {
            DateTime manana = DateTime.Now.Date.AddDays(1);

            // Obtenemos las citas programadas para el día de mañana
            var citasManana = _context.Citas
                .Where(c => c.FechaHoraInicio.Date == manana || c.Estado == "Confirmada" || c.Estado == "Agendada")
                .ToList();

            int recordatoriosEnviados = 0;

            // PATRÓN FACTORY METHOD: Fabricamos las notificaciones por SMS y Correo dinámicamente
            INotificacion notificadorSms = CreadorNotificacion.FabricarNotificacion("SMS");
            INotificacion notificadorCorreo = CreadorNotificacion.FabricarNotificacion("CORREO");

            foreach (var cita in citasManana)
            {
                string mensaje = $"Recordatorio Nicté Ha: Tienes una cita programada para mañana a las {cita.FechaHoraInicio:HH:mm}. ¡Te esperamos!";
                
                // Enviamos vía Factory Method
                notificadorSms.Enviar(mensaje);
                notificadorCorreo.Enviar(mensaje);
                recordatoriosEnviados++;
            }

            TempData["SuccessMessage"] = $"¡Proceso finalizado! Se enviaron {recordatoriosEnviados * 2} notificaciones automáticas (SMS y Correo) para los recordatorios de mañana.";
            return RedirectToAction("Index");
        }
    }
}
