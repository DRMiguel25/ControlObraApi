//EStimacionCosto
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ControlObraApi.Models
{
    public class EstimacionCosto
    {
        [Key]
        public int CostoID { get; set; }
        
        [Required]
        [MaxLength(200)]
        public required string Concepto { get; set; }
        
        // Alta precisión para montos financieros
        [Required]
        [Column(TypeName = "decimal(18,2)")] 
        public decimal MontoEstimado { get; set; }
        
        // Clave Foránea a Proyecto
        [Required]
        public int ProyectoID { get; set; }
        
        public Proyecto? Proyecto { get; set; }

        // Clave de Concurrencia (RowVersion) para Optimistic Locking
        [Timestamp]
        public byte[]? RowVersion { get; set; }

        public ICollection<AvanceObra>? Avances { get; set; }
    }
}
