using Microsoft.EntityFrameworkCore;
using MiSalonBellezaNicteHa.Models;

namespace MiSalonBellezaNicteHa.Data
{
    // Heredamos de DbContext, que es una clase de Entity Framework para manejar bases de datos
    public class SalonDbContext : DbContext
    {
        public SalonDbContext(DbContextOptions<SalonDbContext> options) : base(options)
        {
        }

        // Aquí le ordenamos al sistema que convierta nuestras clases en tablas de SQL
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Estilista> Estilistas { get; set; }
        public DbSet<Servicio> Servicios { get; set; }
        public DbSet<Cita> Citas { get; set; }
    }
}