using System;
using System.Collections.Generic;

namespace MiSalonBellezaNicteHa.Patrones.Observer
{
    // =========================================================================================
    // PATRÓN DE DISEÑO: OBSERVER (Observador)
    // Principio SOLID: OCP (Open/Closed Principle) - Nuevos observadores pueden agregarse sin alterar la clase sujeto.
    // =========================================================================================

    /// <summary>
    /// Interfaz del Observador que define el contrato de notificación de cambio de estado de cita.
    /// </summary>
    public interface IObservadorCita
    {
        void Actualizar(int citaId, string nuevoEstado, string detalle);
    }

    /// <summary>
    /// Observador concreto que envía una notificación SMS al cliente tras el cambio de la cita.
    /// Principio SOLID: SRP (Single Responsibility Principle) - Responsable únicamente del envío de SMS.
    /// </summary>
    public class ObservadorSmsCliente : IObservadorCita
    {
        public void Actualizar(int citaId, string nuevoEstado, string detalle)
        {
            Console.WriteLine($"[SMS ENVIADO AL CLIENTE]: Su cita #{citaId} ha cambiado de estado a '{nuevoEstado}'. Detalle: {detalle}");
        }
    }

    /// <summary>
    /// Observador concreto que envía un correo electrónico de confirmación o cancelación.
    /// </summary>
    public class ObservadorCorreoCliente : IObservadorCita
    {
        public void Actualizar(int citaId, string nuevoEstado, string detalle)
        {
            Console.WriteLine($"[CORREO ENVIADO]: Cita #{citaId} registrada en estado '{nuevoEstado}'. {detalle}");
        }
    }

    /// <summary>
    /// Observador concreto que registra la acción en la bitácora del Administrador.
    /// </summary>
    public class ObservadorBitacoraAdmin : IObservadorCita
    {
        public void Actualizar(int citaId, string nuevoEstado, string detalle)
        {
            Console.WriteLine($"[BITÁCORA ADMIN]: Registro de auditoría guardado. Cita #{citaId} -> {nuevoEstado}");
        }
    }

    /// <summary>
    /// Sujeto o Publicador (Subject) que mantiene la lista de observadores y los notifica.
    /// </summary>
    public class SujetoCita
    {
        private readonly List<IObservadorCita> _observadores = new List<IObservadorCita>();

        public void Suscribir(IObservadorCita observador)
        {
            if (!_observadores.Contains(observador))
            {
                _observadores.Add(observador);
            }
        }

        public void Desuscribir(IObservadorCita observador)
        {
            _observadores.Remove(observador);
        }

        public void Notificar(int citaId, string nuevoEstado, string detalle)
        {
            foreach (var observador in _observadores)
            {
                observador.Actualizar(citaId, nuevoEstado, detalle);
            }
        }
    }
}
