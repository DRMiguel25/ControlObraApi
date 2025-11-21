//Proyecto
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControlObraApi.Models
{
    public class Proyecto
    {
        [Key]
        public int ProyectoID { get; set; }
        public required string NombreObra { get; set; }
        public required string Ubicacion { get; set; }
        public DateTime FechaInicio { get; set; } = DateTime.Now;

        // Relación: Un Proyecto puede tener muchas Estimaciones
        public ICollection<EstimacionCosto> Estimaciones { get; set; } = new List<EstimacionCosto>();
    }
}