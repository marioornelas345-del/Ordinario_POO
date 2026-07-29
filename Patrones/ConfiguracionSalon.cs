namespace MiSalonBellezaNicteHa.Patrones.Singleton
{
    // ==========================================
    // PATRÓN DE DISEÑO: SINGLETON
    // Aplicado para manejar la configuración única del horario del salón.
    // ==========================================
    public class ConfiguracionSalon
    {
        // 1. Variable privada estática que guarda la ÚNICA instancia
        private static ConfiguracionSalon _instancia;

        // 2. Constructor privado: Evita que alguien use la palabra "new" en otra parte del código
        private ConfiguracionSalon()
        {
            // Valores por defecto
            HoraApertura = "09:00";
            HoraCierre = "20:00";
        }

        // 3. El "Portal" público de acceso. Si no existe la configuración, la crea. Si ya existe, te la devuelve.
        public static ConfiguracionSalon Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new ConfiguracionSalon();
                }
                return _instancia;
            }
        }

        // Propiedades del horario
        public string HoraApertura { get; set; }
        public string HoraCierre { get; set; }
    }
}