namespace MiSalonBellezaNicteHa.Models
{
    // Abstracción aplicada en clases
    public abstract class Persona
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; } 
        public string Correo { get; set; }
    }
}