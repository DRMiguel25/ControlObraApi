//EstimacionPatchDTO.cs

namespace ControlObraApi.DTOs
{
    public class EstimacionPatchDTO
    {
        // Usamos string? y decimal? para que el controlador sepa
        // que el valor es opcional y debe ignorarse si es null.
        
        public string? Concepto { get; set; }
        public decimal? MontoEstimado { get; set; }
    }
}