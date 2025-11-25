using System.ComponentModel.DataAnnotations;

namespace ControlObraApi.DTOs
{
    public class AvanceObraUpdateDTO
    {
        public int AvanceID { get; set; }

        [Required(ErrorMessage = "El monto ejecutado es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto ejecutado debe ser mayor a cero")]
        public decimal MontoEjecutado { get; set; }

        [Required(ErrorMessage = "El porcentaje completado es obligatorio")]
        [Range(0, 100, ErrorMessage = "El porcentaje debe estar entre 0 y 100")]
        public decimal PorcentajeCompletado { get; set; }

        public int CostoID { get; set; }
    }
}
