//AvanceObra
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControlObraApi.Models
{
    /// <summary>
    /// Representa el avance de ejecución de una estimación de costo
    /// </summary>
    public class AvanceObra
    {
        [Key]
        public int AvanceID { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal MontoEjecutado { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal PorcentajeCompletado { get; set; }

        [Required]
        public DateTime FechaRegistro { get; set; }

        // Foreign Key
        [Required]
        public int CostoID { get; set; }

        // Navegación (opcional - nullable)
        public EstimacionCosto? EstimacionCosto { get; set; }

        // Fotos de evidencia
        public ICollection<AvanceFoto>? Fotos { get; set; }
    }
}
