
//ProyectoPatchDTO
using System;

namespace ControlObraApi.DTOs
{
    /// <summary>
    /// DTO para actualizaciones parciales de Proyecto
    /// </summary>
    public class ProyectoPatchDTO
    {
        public string? NombreObra { get; set; }
        public string? Ubicacion { get; set; }
        public DateTime? FechaInicio { get; set; }
    }
}
