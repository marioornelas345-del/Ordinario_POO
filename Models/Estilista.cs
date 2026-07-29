namespace MiSalonBellezaNicteHa.Models
{
    // Herencia: Estilista hereda de Persona
    public class Estilista : Persona
    {
        public string NumeroEmpleado { get; set; }
        public bool RealizaTodosLosServicios { get; set; } 
        
        public Estilista()
        {
            // Regla del PDF: Cada empleado puede realizar TODOS LOS SERVICIOS
            RealizaTodosLosServicios = true;
        }
    }
}