
//AvanceObraValidator
using FluentValidation;
using ControlObraApi.Models;

namespace ControlObraApi.Validators
{
    /// <summary>
    /// Validador para la entidad AvanceObra
    /// </summary>
    public class AvanceObraValidator : AbstractValidator<AvanceObra>
    {
        public AvanceObraValidator()
        {
            // Regla 1: El monto ejecutado debe ser positivo
            RuleFor(a => a.MontoEjecutado)
                .GreaterThan(0)
                .WithMessage("El monto ejecutado debe ser mayor a cero.");

            // Regla 2: El porcentaje completado debe estar entre 0 y 100
            RuleFor(a => a.PorcentajeCompletado)
                .InclusiveBetween(0, 100)
                .WithMessage("El porcentaje completado debe estar entre 0 y 100.");

            // Regla 3: El CostoID debe ser válido (número positivo)
            RuleFor(a => a.CostoID)
                .GreaterThan(0)
                .WithMessage("Debe estar asociado a un CostoID válido.");

            // NO validamos FechaRegistro porque se asigna automáticamente 
            // en el controlador con DateTime.UtcNow
        }
    }
}
