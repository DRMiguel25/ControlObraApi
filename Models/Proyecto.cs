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

        // 🆕 OWNERSHIP: Relación con Usuario Propietario
        public int UserId { get; set; }
        
        [ForeignKey("UserId")]
        [Newtonsoft.Json.JsonIgnore]
        public User? User { get; set; }

        // Relación: Un Proyecto puede tener muchas Estimaciones
        public ICollection<EstimacionCosto> Estimaciones { get; set; } = new List<EstimacionCosto>();
    }
}
