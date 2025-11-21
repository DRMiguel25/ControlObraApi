//EstimacionCostoCreteDTO
using System.ComponentModel.DataAnnotations;

namespace ControlObraApi.DTOs
{
    /// <summary>
    /// DTO para crear una nueva Estimación de Costo
    /// Solo incluye los campos necesarios para la operación POST
    /// </summary>
    public class EstimacionCostoCreateDTO
    {
        [Required(ErrorMessage = "El concepto es obligatorio")]
        [MaxLength(200, ErrorMessage = "El concepto no puede exceder 200 caracteres")]
        public string Concepto { get; set; }

        [Required(ErrorMessage = "El monto estimado es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto estimado debe ser mayor a cero")]
        public decimal MontoEstimado { get; set; }

        [Required(ErrorMessage = "El ID del proyecto es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del proyecto debe ser válido")]
        public int ProyectoID { get; set; }
    }
}
