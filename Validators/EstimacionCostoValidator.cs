//EstimacionCostoValidator
using FluentValidation;
using ControlObraApi.Models;

namespace ControlObraApi.Validators
{
    // Esta debe ser la única definición de esta clase
    public class EstimacionCostoValidator : AbstractValidator<EstimacionCosto>
    {
        public EstimacionCostoValidator()
        {
            // Regla 1: El concepto no puede estar vacío.
            RuleFor(e => e.Concepto)
                .NotEmpty()
                .WithMessage("El concepto de la estimación no puede estar vacío.")
                .MaximumLength(200);

            // Regla 2: El monto estimado debe ser positivo.
            RuleFor(e => e.MontoEstimado)
                .GreaterThan(0)
                .WithMessage("El monto estimado debe ser mayor a cero.");

            // Regla 3: El ProyectoID debe ser válido (un número positivo).
            RuleFor(e => e.ProyectoID)
                .GreaterThan(0)
                .WithMessage("Debe estar asociado a un ProyectoID válido.");
        }
    }
}