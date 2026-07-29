using Microsoft.AspNetCore.Mvc;
using MiSalonBellezaNicteHa.Models;
using System.Collections.Generic;
using System.Linq;

namespace MiSalonBellezaNicteHa.Controllers
{
    /// <summary>
    /// Controlador que gestiona el catálogo visual de servicios del salón.
    /// Proporciona la información completa de precios, fotografías descriptivas
    /// e información detallada de cada corte/servicio.
    /// </summary>
    public class ServiciosController : Controller
    {
        // -----------------------------------------------------------------------------
        // Catálogo principal de servicios almacenado en memoria con fotografías e información detallada
        // -----------------------------------------------------------------------------
        private static readonly List<Servicio> CatálogoServicios = new List<Servicio>
        {
            // === SERVICIOS DE HOMBRES (CATEGORÍA "H") ===
            new Servicio 
            { 
                Id = 1, 
                Nombre = "Corte Clásico de Hombre", 
                Precio = 120, 
                CategoriaGenero = "H",
                ImagenUrl = "https://ramalloarthair.com.ar/wp-content/uploads/2017/05/tendencia-corte-pelo-hombres-1.png",
                DescripcionDetallada = "Corte tradicional a máquina y tijera adaptado a la forma de tu rostro. Incluye lavado capilar con champú refrescante, perfilado con navaja en patillas y nuca, y peinado final con cera o pomada mate profesional.",
                DuracionEstimada = "35 min"
            },
            new Servicio 
            { 
                Id = 2, 
                Nombre = "Arreglo y Perfilado de Barba", 
                Precio = 100, 
                CategoriaGenero = "H",
                ImagenUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcT-sVwQ0VqVAvSsXnDuKLmvHVMw3UkujBcxUv1BakU_R_Q-ivf2ygAIQHVC&s=10",
                DescripcionDetallada = "Recorte y diseño de barba con tratamiento de toalla caliente para abrir poros, aplicación de aceites hidratantes y rasurado de líneas de mejillas y cuello con navaja libre.",
                DuracionEstimada = "30 min"
            },
            new Servicio 
            { 
                Id = 3, 
                Nombre = "Combo: Corte de Cabello + Barba", 
                Precio = 170, 
                CategoriaGenero = "H",
                ImagenUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSBDnxgqAbz-WWdJyPOuyz7a1EMPV3ilJ9H6N9qoWFVNtzwpsmxbuz_AeIe&s=10",
                DescripcionDetallada = "Servicio integral de barbería. Incluye el corte de cabello completo a máquina y tijera + perfilado completo de barba con toalla caliente, masaje capilar y productos de acabado de alta gama.",
                DuracionEstimada = "1 hora"
            },

            // === SERVICIOS DE MUJERES (CATEGORÍA "M") ===
            new Servicio 
            { 
                Id = 4, 
                Nombre = "Corte de Cabello para Mujer", 
                Precio = 120, 
                CategoriaGenero = "M",
                ImagenUrl = "https://www.peluqueriacristinacisneros.es/wp-content/uploads/2025/12/cortes-pelo-2026-tendencia.png",
                DescripcionDetallada = "Corte de cabello femenino personalizado (puntas, capas, estilo Bob o degrafilado). Incluye diagnóstico de hidratación, lavado relajante y secado moldeado.",
                DuracionEstimada = "45 min"
            },
            new Servicio 
            { 
                Id = 5, 
                Nombre = "Aplicación de Uñas / Manicura", 
                Precio = 250, 
                CategoriaGenero = "M",
                ImagenUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSQVbLEwOmVIx81v5eY1uJBXdb9lEEvuSzJyQ0UySNQ8bEi4koHlJ90HVFs&s=10",
                DescripcionDetallada = "Manicura estética completa con limado, retiro de cutícula, exfoliación suave y aplicación de uñas acrílicas o esmaltado en Gelish de larga duración con diseño a elegir.",
                DuracionEstimada = "1 hora 15 min"
            },
            new Servicio 
            { 
                Id = 6, 
                Nombre = "Maquillaje Profesional", 
                Precio = 200, 
                CategoriaGenero = "M",
                ImagenUrl = "https://www.shutterstock.com/image-photo/makeup-artist-applies-eye-shadow-260nw-2698447743.jpg",
                DescripcionDetallada = "Maquillaje social de alta definición (HD) resistente al agua y sudor. Incluye preparación e hidratación de piel, diseño de cejas, sombreado de ojos e incluye pestañas postizas de tira o punto.",
                DuracionEstimada = "50 min"
            },
            new Servicio 
            { 
                Id = 7, 
                Nombre = "Peinados y Peinado Social", 
                Precio = 130, 
                CategoriaGenero = "M",
                ImagenUrl = "https://estefanialastrahairbeauty.com/wp-content/uploads/2026/02/8c419146-3f0d-442b-9de5-2ceb30ffe865-683x1024.png",
                DescripcionDetallada = "Peinado para eventos sociales, fiestas o graduaciones (recogidos, semi-recogidos, ondas al agua o alaciado express). Fijación garantizada con sprays de alta calidad.",
                DuracionEstimada = "45 min"
            }
        };

        // -----------------------------------------------------------------------------
        // ACCIÓN: Index (Muestra las tarjetas de servicios y maneja el filtro "Todos", "H" y "M")
        // -----------------------------------------------------------------------------
        public IActionResult Index(string filtro = "Todos")
        {
            ViewData["FiltroActual"] = filtro;
            var resultados = CatálogoServicios.AsEnumerable();

            if (filtro == "H")
            {
                resultados = resultados.Where(s => s.CategoriaGenero == "H");
            }
            else if (filtro == "M")
            {
                resultados = resultados.Where(s => s.CategoriaGenero == "M");
            }

            // Enviamos la lista filtrada a la vista Razor
            return View("~/Views/Servicios/Index.cshtml", resultados.ToList());
        }

        // -----------------------------------------------------------------------------
        // ACCIÓN: ObtenerServicioPorId (Retorna los datos de un servicio específico)
        // -----------------------------------------------------------------------------
        public static Servicio ObtenerServicioPorId(int id)
        {
            return CatálogoServicios.FirstOrDefault(s => s.Id == id) ?? CatálogoServicios.First();
        }
    }
}