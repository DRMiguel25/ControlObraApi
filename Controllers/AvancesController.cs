//AvancesControllers
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ControlObraApi.Models;
using ControlObraApi.DTOs;
using FluentValidation;
using FluentValidation.Results;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace ControlObraApi.Controllers
{
    [Route("api/Avances")]
    [ApiController]
    public class AvancesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IValidator<AvanceObra> _validator;

        // Constructor con inyección de dependencias
        public AvancesController(AppDbContext context, IValidator<AvanceObra> validator)
        {
            _context = context;
            _validator = validator;
        }

        // -----------------------------------------------------------------
        // POST: Registrar Avance de Obra (C - CREATE) - CON DTO
        // -----------------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> PostAvanceObra([FromBody] AvanceObraCreateDTO dto)
        {
            // 1. Convertir DTO a entidad
            var avance = new AvanceObra
            {
                MontoEjecutado = dto.MontoEjecutado,
                PorcentajeCompletado = dto.PorcentajeCompletado,
                CostoID = dto.CostoID,
                FechaRegistro = DateTime.Now // Asignar fecha automáticamente
            };

            // 2. Validación con FluentValidation
            ValidationResult validationResult = await _validator.ValidateAsync(avance);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            // 3. Guardar en base de datos
            _context.AvancesObra.Add(avance);
            await _context.SaveChangesAsync();

            // 4. Retorno 201 Created
            return CreatedAtAction(
                nameof(GetAvanceObra), 
                new { id = avance.AvanceID }, 
                avance);
        }

        // -----------------------------------------------------------------
        // GET: Buscar Avance por ID (R - READ)
        // -----------------------------------------------------------------
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAvanceObra(int id)
        {
            // Incluye la relación con EstimacionCosto para ver el contexto completo
            var avance = await _context.AvancesObra
                .Include(a => a.EstimacionCosto)
                .FirstOrDefaultAsync(a => a.AvanceID == id);

            if (avance == null)
            {
                return NotFound($"Avance con ID {id} no encontrado.");
            }

            return Ok(avance);
        }

        // -----------------------------------------------------------------
        // GET: Listar todos los Avances (R - READ ALL)
        // -----------------------------------------------------------------
        [HttpGet]
        public async Task<IActionResult> GetAllAvances()
        {
            var avances = await _context.AvancesObra
                .Include(a => a.EstimacionCosto)
                .ToListAsync();

            return Ok(avances);
        }

        // -----------------------------------------------------------------
        // PUT: Actualización Completa (U - UPDATE)
        // -----------------------------------------------------------------
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAvanceObra(int id, [FromBody] AvanceObra avance)
        {
            if (id != avance.AvanceID)
            {
                return BadRequest("El ID de la ruta no coincide con el ID del cuerpo.");
            }

            // Validación
            ValidationResult validationResult = await _validator.ValidateAsync(avance);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }

            _context.Attach(avance).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.AvancesObra.Any(a => a.AvanceID == id))
                {
                    return NotFound("Avance no encontrado.");
                }
                return StatusCode(409, "Conflicto de Concurrencia.");
            }

            return NoContent(); // 204 No Content
        }

        // -----------------------------------------------------------------
        // PATCH: Actualización Parcial (U - UPDATE Parcial)
        // -----------------------------------------------------------------
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchAvanceObra(int id, [FromBody] AvancePatchDTO patchDto)
        {
            var avance = await _context.AvancesObra.FindAsync(id);

            if (avance == null)
            {
                return NotFound($"Avance con ID {id} no encontrado.");
            }

            // Aplicar solo los campos que vienen en el DTO
            if (patchDto.MontoEjecutado.HasValue)
            {
                if (patchDto.MontoEjecutado.Value <= 0)
                {
                    return BadRequest("El monto ejecutado debe ser mayor a cero.");
                }
                avance.MontoEjecutado = patchDto.MontoEjecutado.Value;
            }

            if (patchDto.PorcentajeCompletado.HasValue)
            {
                if (patchDto.PorcentajeCompletado.Value < 0 || patchDto.PorcentajeCompletado.Value > 100)
                {
                    return BadRequest("El porcentaje debe estar entre 0 y 100.");
                }
                avance.PorcentajeCompletado = patchDto.PorcentajeCompletado.Value;
            }

            await _context.SaveChangesAsync();

            return Ok(avance);
        }

        // -----------------------------------------------------------------
        // DELETE: Eliminar Avance (D - DELETE)
        // -----------------------------------------------------------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAvanceObra(int id)
        {
            var avance = await _context.AvancesObra.FindAsync(id);

            if (avance == null)
            {
                return NotFound($"Avance con ID {id} no encontrado.");
            }

            _context.AvancesObra.Remove(avance);
            await _context.SaveChangesAsync();

            return NoContent(); // 204 No Content
        }

        // -----------------------------------------------------------------
        // GET: Obtener Avances por Estimación (Query personalizado)
        // -----------------------------------------------------------------
        [HttpGet("porEstimacion/{costoId}")]
        public async Task<IActionResult> GetAvancesPorEstimacion(int costoId)
        {
            var avances = await _context.AvancesObra
                .Where(a => a.CostoID == costoId)
                .Include(a => a.EstimacionCosto)
                .ToListAsync();

            if (!avances.Any())
            {
                return NotFound($"No se encontraron avances para la estimación {costoId}.");
            }

            return Ok(avances);
        }
    }
}
