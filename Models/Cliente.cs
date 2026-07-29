namespace MiSalonBellezaNicteHa.Models
{
    // Herencia: Cliente hereda de Persona
    public class Cliente : Persona
    {
        public string Telefono { get; set; }
        public string RutaFotografia { get; set; } // Foto del usuario que pide el PDF
        
        public Cliente()
        {
            RutaFotografia = "sin-foto.jpg";
        }
    }
}