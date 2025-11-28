using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ControlObraApi.Models
{
    public class AvanceFoto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Url { get; set; } = string.Empty;

        public string Orientacion { get; set; } = "Horizontal"; // "Horizontal" or "Vertical"

        [ForeignKey("AvanceObra")]
        public int AvanceObraId { get; set; }

        [JsonIgnore]
        public virtual AvanceObra? AvanceObra { get; set; }
    }
}
