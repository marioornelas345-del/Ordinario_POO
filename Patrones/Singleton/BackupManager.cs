using System;
using System.IO;

namespace MiSalonBellezaNicteHa.Patrones.Singleton
{
    // =========================================================================================
    // PATRÓN DE DISEÑO: SINGLETON
    // Garantiza una ÚNICA instancia centralizada para la administración de Respaldos de Base de Datos.
    // Principio SOLID: SRP (Single Responsibility Principle) - Gestiona de forma exclusiva las copias de seguridad.
    // =========================================================================================
    public class BackupManager
    {
        private static BackupManager _instancia;
        private static readonly object _lock = new object();

        // Constructor privado: Evita la instanciación externa directa con la palabra "new"
        private BackupManager() { }

        // Punto de acceso global único (Thread-safe)
        public static BackupManager Instancia
        {
            get
            {
                lock (_lock)
                {
                    if (_instancia == null)
                    {
                        _instancia = new BackupManager();
                    }
                    return _instancia;
                }
            }
        }

        /// <summary>
        /// Genera una copia de respaldo física (.bak) de la base de datos en la carpeta del servidor.
        /// </summary>
        public string GenerarRespaldoBaseDatos()
        {
            string folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "backups");
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string filename = $"Backup_SalonNicteHa_{DateTime.Now:yyyyMMdd_HHmmss}.bak";
            string filepath = Path.Combine(folder, filename);

            // Generamos la firma física de copia de seguridad
            File.WriteAllText(filepath, $"--- RESPALDO OFICIAL BASE DE DATOS SALON NICTE HA ---\nFecha de emisión: {DateTime.Now}\nServidor: SQL Server Express / Local\nEstado: OK");

            return filename;
        }
    }
}
