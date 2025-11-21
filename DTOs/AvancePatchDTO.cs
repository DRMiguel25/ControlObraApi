//AvanceObraCreateDTO
namespace ControlObraApi.DTOs
{
    /// <summary>
    /// DTO para actualizaciones parciales (PATCH) de Avance de Obra
    /// Todos los campos son opcionales (nullable)
    /// </summary>
    public class AvancePatchDTO
    {
        public decimal? MontoEjecutado { get; set; }
        
        public decimal? PorcentajeCompletado { get; set; }
    }
}
