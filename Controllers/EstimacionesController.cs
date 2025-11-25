using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ControlObraApi.Models;
using ControlObraApi.DTOs;
using FluentValidation;
using FluentValidation.Results;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ControlObraApi.Controllers
{
    [Route("api/Estimaciones")]
    [ApiController]
    [Authorize]
    public class EstimacionesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IValidator<EstimacionCosto> _validator;

        public EstimacionesController(AppDbContext context, IValidator<EstimacionCosto> validator)
        {
            _context = context;
            _validator = validator;
        }

        // 🆕 Helper: Obtener ID del usuario autenticado
        private int GetCurrentUserId()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        }

        // 🆕 Helper: Validar que el usuario sea dueño del proyecto
        private async Task<bool> UserOwnsProyectoAsync(int proyectoId)
        {
            var userId = GetCurrentUserId();
            return await _context.Proyectos.AnyAsync(p => p.ProyectoID == proyectoId && p.UserId == userId);
        }

        // -----------------------------------------------------------------
        // POST: Registrar Presupuesto (C - CREATE) - CON DTO
        // -----------------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> PostEstimacionCosto([FromBody] EstimacionCostoCreateDTO dto)
        {
            // 🆕 Validar ownership del proyecto
            if (!await UserOwnsProyectoAsync(dto.ProyectoID))
            {
                return Forbid();  // 403 - No eres dueño del proyecto
            }

            var estimacion = new EstimacionCosto
            {
                Concepto = dto.Concepto,
                MontoEstimado = dto.MontoEstimado,
                ProyectoID = dto.ProyectoID
            };

            ValidationResult validationResult = await _validator.ValidateAsync(estimacion);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            _context.EstimacionesCosto.Add(estimacion);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetEstimacionCosto), 
                new { id = estimacion.CostoID }, 
                estimacion);
        }
        
        // -----------------------------------------------------------------
        // GET: Buscar Estimacion por ID (R - READ)
        // CORRECCIÓN CLAVE: Se especifica que el ID debe ser un entero
        // -----------------------------------------------------------------
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetEstimacionCosto(int id)
        {
            var estimacion = await _context.EstimacionesCosto
                .Include(e => e.Proyecto)  // 🆕 Include para validar ownership
                .Include(e => e.Avances) 
                .FirstOrDefaultAsync(e => e.CostoID == id);

            if (estimacion == null)
            {
                return NotFound();
            }

            // 🆕 Validar ownership via proyecto
            if (!await UserOwnsProyectoAsync(estimacion.ProyectoID))
            {
                return Forbid();
            }

            return Ok(estimacion);
        }

        // -----------------------------------------------------------------
        // PUT: Actualizacion Completa (U - UPDATE Total)
        // CORRECCIÓN CLAVE: Se especifica que el ID debe ser un entero
        // -----------------------------------------------------------------
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutEstimacionCosto(int id, [FromBody] EstimacionCosto estimacion)
        {
            if (id != estimacion.CostoID)
            {
                return BadRequest("El ID de la ruta no coincide con el ID del cuerpo.");
            }
            
            // 🆕 Validar ownership antes de actualizar
            if (!await UserOwnsProyectoAsync(estimacion.ProyectoID))
            {
                return Forbid();
            }
            
            ValidationResult validationResult = await _validator.ValidateAsync(estimacion);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }
            
            _context.Attach(estimacion).State = EntityState.Modified;
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.EstimacionesCosto.Any(e => e.CostoID == id))
                {
                    return NotFound("Estimación no encontrada.");
                }
                return StatusCode(409, "Conflicto de Concurrencia: El registro fue modificado por otro usuario."); 
            }

            return NoContent();
        }

        // -----------------------------------------------------------------
        // PATCH: Ajuste Parcial de Presupuesto (U - Eficiente con DTO)
        // CORRECCIÓN CLAVE: Se especifica que el ID debe ser un entero
        // -----------------------------------------------------------------
        [HttpPatch("{id:int}")]
        public async Task<IActionResult> PatchEstimacionCosto(int id, [FromBody] EstimacionPatchDTO patchDto)
        {
            var estimacionModelo = await _context.EstimacionesCosto.FindAsync(id);

            if (estimacionModelo == null)
            {
                return NotFound($"Estimación con ID {id} no encontrada.");
            }
            
            // 🆕 Validar ownership
            if (!await UserOwnsProyectoAsync(estimacionModelo.ProyectoID))
            {
                return Forbid();
            }
            
            if (patchDto.Concepto != null)
            {
                estimacionModelo.Concepto = patchDto.Concepto;
            }

            if (patchDto.MontoEstimado.HasValue) 
            {
                if (patchDto.MontoEstimado.Value <= 0)
                {
                    return BadRequest("El monto estimado no puede ser menor o igual a cero.");
                }
                estimacionModelo.MontoEstimado = patchDto.MontoEstimado.Value;
            }
            
            await _context.SaveChangesAsync();

            return Ok(estimacionModelo);
        }

        // -----------------------------------------------------------------
        // DELETE: Eliminar Estimacion (CON VALIDACION MEJORADA)
        // CORRECCIÓN CLAVE: Se especifica que el ID debe ser un entero
        // -----------------------------------------------------------------
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteEstimacionCosto(int id)
        {
            var estimacion = await _context.EstimacionesCosto
                .Include(e => e.Avances)
                .FirstOrDefaultAsync(e => e.CostoID == id);

            if (estimacion == null)
            {
                return NotFound($"Estimación con ID {id} no encontrada.");
            }

            // 🆕 Validar ownership
            if (!await UserOwnsProyectoAsync(estimacion.ProyectoID))
            {
                return Forbid();
            }

            // Validar que no tenga avances
            if (estimacion.Avances != null && estimacion.Avances.Any())
            {
                return BadRequest(new 
                { 
                    error = "No se puede eliminar la estimación",
                    razon = "La estimación tiene avances de obra registrados. Elimínelos primero.",
                    avancesCount = estimacion.Avances.Count
                });
            }

            _context.EstimacionesCosto.Remove(estimacion);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}