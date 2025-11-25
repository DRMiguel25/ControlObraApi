using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ControlObraApi.Models;
using ControlObraApi.DTOs;
using FluentValidation;
using FluentValidation.Results;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ControlObraApi.Controllers
{
    [Route("api/Avances")]
    [ApiController]
    [Authorize]
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

        // 🆕 Helper: Obtener ID del usuario autenticado
        private int GetCurrentUserId()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        }

        // 🆕 Helper: Validar ownership via EstimacionCosto -> Proyecto
        private async Task<bool> UserOwnsEstimacionAsync(int costoId)
        {
            var userId = GetCurrentUserId();
            return await _context.EstimacionesCosto
                .Include(e => e.Proyecto)
                .AnyAsync(e => e.CostoID == costoId && e.Proyecto.UserId == userId);
        }

        // -----------------------------------------------------------------
        // POST: Registrar Avance de Obra (C - CREATE) - CON DTO
        // -----------------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> PostAvanceObra([FromBody] AvanceObraCreateDTO dto)
        {
            // 🆕 Validar ownership de la estimación
            if (!await UserOwnsEstimacionAsync(dto.CostoID))
            {
                return Forbid();
            }

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
        // CORRECCIÓN CLAVE: Se especifica que el ID debe ser un entero
        // -----------------------------------------------------------------
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAvanceObra(int id)
        {
            // Incluye la relación con EstimacionCosto para ver el contexto completo
            var avance = await _context.AvancesObra
                .Include(a => a.EstimacionCosto)
                    .ThenInclude(e => e.Proyecto)
                .FirstOrDefaultAsync(a => a.AvanceID == id);

            if (avance == null)
            {
                return NotFound($"Avance con ID {id} no encontrado.");
            }

            // 🆕 Validar ownership
            if (!await UserOwnsEstimacionAsync(avance.CostoID))
            {
                return Forbid();
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
        // CORRECCIÓN CLAVE: Se especifica que el ID debe ser un entero
        // -----------------------------------------------------------------
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutAvanceObra(int id, [FromBody] AvanceObraUpdateDTO dto)
        {
            if (id != dto.AvanceID)
            {
                return BadRequest("El ID de la ruta no coincide con el ID del cuerpo.");
            }

            // 1. Buscar la entidad existente
            var avanceExistente = await _context.AvancesObra.FindAsync(id);
            if (avanceExistente == null)
            {
                return NotFound("Avance no encontrado.");
            }

            // 2. Validar ownership (seguridad)
            if (!await UserOwnsEstimacionAsync(avanceExistente.CostoID))
            {
                return Forbid();
            }

            // 3. Actualizar campos
            avanceExistente.MontoEjecutado = dto.MontoEjecutado;
            avanceExistente.PorcentajeCompletado = dto.PorcentajeCompletado;
            // No actualizamos FechaRegistro ni CostoID en un PUT estándar, 
            // o si se requiere, se puede actualizar CostoID si es válido.
            
            // 4. Guardar cambios
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
                throw;
            }

            return NoContent(); // 204 No Content
        }

        // -----------------------------------------------------------------
        // PATCH: Actualización Parcial (U - UPDATE Parcial)
        // CORRECCIÓN CLAVE: Se especifica que el ID debe ser un entero
        // -----------------------------------------------------------------
        [HttpPatch("{id:int}")]
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
        // CORRECCIÓN CLAVE: Se especifica que el ID debe ser un entero
        // -----------------------------------------------------------------
        [HttpDelete("{id:int}")]
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
        // CORRECCIÓN CLAVE: Se especifica que el ID debe ser un entero
        // -----------------------------------------------------------------
        [HttpGet("porEstimacion/{costoId:int}")]
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