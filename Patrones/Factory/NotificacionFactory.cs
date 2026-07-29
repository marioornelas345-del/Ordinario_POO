using System;

namespace MiSalonBellezaNicteHa.Patrones.Factory
{
    // 1. Creamos un "molde" o regla de lo que debe hacer cualquier notificación
    public interface INotificacion
    {
        void Enviar(string mensaje);
    }

    // 2. Creamos la notificación por SMS que sigue la regla
    public class NotificacionSms : INotificacion
    {
        public void Enviar(string mensaje)
        {
            // Aquí iría el código real para mandar un SMS a un teléfono
            Console.WriteLine("Enviando SMS al cliente: " + mensaje);
        }
    }

    // 3. Creamos la notificación por Correo que sigue la misma regla
    public class NotificacionCorreo : INotificacion
    {
        public void Enviar(string mensaje)
        {
            // Aquí iría el código real para mandar un email
            Console.WriteLine("Enviando Correo al cliente: " + mensaje);
        }
    }

    // ==========================================
    // PATRÓN DE DISEÑO: FACTORY METHOD
    // Aplicado para fabricar el tipo de notificación correcto según lo que pida el usuario.
    // ==========================================
    public class CreadorNotificacion
    {
        // Esta es la "Fábrica". Le pasas la palabra "SMS" o "Correo" y te devuelve el objeto correcto.
        public static INotificacion FabricarNotificacion(string tipo)
        {
            if (tipo.ToUpper() == "SMS")
            {
                return new NotificacionSms();
            }
            else if (tipo.ToUpper() == "CORREO")
            {
                return new NotificacionCorreo();
            }
            else
            {
                throw new ArgumentException("Tipo de notificación no válido");
            }
        }
    }
}