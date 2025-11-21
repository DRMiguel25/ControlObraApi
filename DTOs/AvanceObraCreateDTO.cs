//AvanceObraCreateDTO
using System.ComponentModel.DataAnnotations;

namespace ControlObraApi.DTOs
{
    /// <summary>
    /// DTO para crear un nuevo Avance de Obra
    /// Solo incluye los campos que el usuario debe enviar
    /// </summary>
    public class AvanceObraCreateDTO
    {
        [Required(ErrorMessage = "El monto ejecutado es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto ejecutado debe ser mayor a cero")]
        public decimal MontoEjecutado { get; set; }

        [Required(ErrorMessage = "El porcentaje completado es obligatorio")]
        [Range(0, 100, ErrorMessage = "El porcentaje debe estar entre 0 y 100")]
        public decimal PorcentajeCompletado { get; set; }

        [Required(ErrorMessage = "El ID de la estimación de costo es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID de la estimación debe ser válido")]
        public int CostoID { get; set; }
    }
}
