namespace MiSalonBellezaNicteHa.Models
{
    /// <summary>
    /// Clase Modelo que representa un servicio del Salón Nicté Ha.
    /// Incluye propiedades para la imagen, descripción extendida y categoría de género.
    /// </summary>
    public class Servicio
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        
        // Categoría para filtrado: "H" (Caballeros), "M" (Damas)
        public string CategoriaGenero { get; set; } = string.Empty; 

        // URL de la fotografía descriptiva del servicio
        public string ImagenUrl { get; set; } = string.Empty;

        // Descripción detallada del procedimiento y qué incluye
        public string DescripcionDetallada { get; set; } = string.Empty;

        // Tiempo estimado de atención (ej. "45 min", "1 hora")
        public string DuracionEstimada { get; set; } = "45 min";
    }
}