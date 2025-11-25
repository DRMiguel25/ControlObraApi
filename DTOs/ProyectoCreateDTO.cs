using System.ComponentModel.DataAnnotations;

namespace ControlObraApi.DTOs
{
    public class ProyectoCreateDTO
    {
        [Required(ErrorMessage = "El nombre de la obra es obligatorio")]
        public string NombreObra { get; set; } = string.Empty;

        [Required(ErrorMessage = "La ubicación es obligatoria")]
        public string Ubicacion { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
        public string FechaInicio { get; set; } = string.Empty;
    }
}
