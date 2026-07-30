using Microsoft.AspNetCore.Mvc;
using MiSalonBellezaNicteHa.Data;
using MiSalonBellezaNicteHa.Models;
using System;
using System.Linq;

namespace MiSalonBellezaNicteHa.Controllers
{
    /// <summary>
    /// Controlador para la gestión de citas y conexión fluida hacia el sistema de pagos.
    /// </summary>
    public class CitasController : Controller
    {
        private readonly SalonDbContext _context;

        public CitasController(SalonDbContext context)
        {
            _context = context;
        }

        // -----------------------------------------------------------------------------
        // ACCIÓN: Index (Punto de entrada general para Citas)
        // -----------------------------------------------------------------------------
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("UsuarioLogueado") == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // -----------------------------------------------------------------------------
        // ACCIÓN: Registrar (Abre el formulario para agendar cita, opcionalmente con un servicio pre-seleccionado)
        // -----------------------------------------------------------------------------
        public IActionResult Registrar(int servicioId = 1)
        {
            if (HttpContext.Session.GetString("UsuarioLogueado") == null)
            {
                return RedirectToAction("Index", "Home");
            }
            // Obtenemos los datos del servicio seleccionado para pre-llenar los precios en la vista
            var servicio = ServiciosController.ObtenerServicioPorId(servicioId);
            ViewData["ServicioSeleccionadoId"] = servicio.Id;
            ViewData["NombreServicio"] = servicio.Nombre;
            ViewData["PrecioServicio"] = servicio.Precio;

            return View("~/Views/Citas/Registrar.cshtml");
        }

        // -----------------------------------------------------------------------------
        // ACCIÓN: AgendarTurno (Guarda la cita y redirige AL SEGUNDO PASO: Pagar la cita)
        // -----------------------------------------------------------------------------
        [HttpPost]
        public IActionResult AgendarTurno(int clienteId, int servicioId, int estilistaId, DateTime horaElegida)
        {
            // 1. Obtenemos la información del servicio elegido
            var servicioObj = ServiciosController.ObtenerServicioPorId(servicioId);

            // 2. Creamos y configuramos la nueva Cita
            var nuevaCita = new Cita 
            { 
                ClienteId = clienteId > 0 ? clienteId : 1,
                ServicioId = servicioId,
                EstilistaId = estilistaId > 0 ? estilistaId : 1
            };

            // Regla de negocio: Asignamos el horario con duración automática de 1 hora
            nuevaCita.AsignarHorario(horaElegida != default ? horaElegida : DateTime.Now.AddDays(1));

            // Guardamos en la base de datos SQL Server
            _context.Citas.Add(nuevaCita);
            _context.SaveChanges();

            // 3. FLUJO CONECTADO: Mandamos al usuario a la pantalla de Pagos con los datos de su cita
            return RedirectToAction("Index", "Pagos", new { 
                citaId = nuevaCita.Id, 
                monto = servicioObj.Precio, 
                servicio = servicioObj.Nombre 
            });
        }

        // -----------------------------------------------------------------------------
        // ACCIÓN: AgendaDiaria (Muestra la lista de citas agendadas)
        // -----------------------------------------------------------------------------
        public IActionResult AgendaDiaria()
        {
            var citas = _context.Citas.ToList();
            return View("~/Views/Citas/AgendaDiaria.cshtml", citas);
        }
    }
}