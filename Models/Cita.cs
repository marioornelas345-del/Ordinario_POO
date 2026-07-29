
using System;

namespace MiSalonBellezaNicteHa.Models
{
    public class Cita
    {
        public int Id { get; set; }
        public DateTime FechaHoraInicio { get; set; }
        
        // Encapsulamiento: Solo se puede leer, no se puede cambiar a mano
        public DateTime FechaHoraFin { get; private set; } 
        public string Estado { get; set; } 

        // Conexiones con las otras clases
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
        
        public int EstilistaId { get; set; }
        public Estilista Estilista { get; set; }

        public int ServicioId { get; set; }
        public Servicio Servicio { get; set; }

        public Cita()
        {
            Estado = "Agendada";
        }

        // Abstracción aplicada en métodos: 
        // Escondemos el cálculo de la hora para cumplir la regla del examen
        public void AsignarHorario(DateTime horaInicio)
        {
            FechaHoraInicio = horaInicio;
            // Sumamos exactamente 1 hora automática (Regla del PDF)
            FechaHoraFin = horaInicio.AddHours(1); 
        }
    }
}