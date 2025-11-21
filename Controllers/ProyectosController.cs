//ProyectosController
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ControlObraApi.Models;
using ControlObraApi.DTOs;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ControlObraApi.Controllers
{
    [Route("api/Proyectos")]
    [ApiController]
    public class ProyectosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProyectosController(AppDbContext context)
        {
            _context = context;
        }

        // -----------------------------------------------------------------
        // GET: Obtener Proyecto por ID con sus Estimaciones
        // -----------------------------------------------------------------
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProyecto(int id)
        {
            var proyecto = await _context.Proyectos
                .Include(p => p.Estimaciones)
                    .ThenInclude(e => e.Avances)
                .FirstOrDefaultAsync(p => p.ProyectoID == id);

            if (proyecto == null)
            {
                return NotFound($"Proyecto con ID {id} no encontrado.");
            }

            return Ok(proyecto);
        }

        // -----------------------------------------------------------------
        // GET: Obtener todos los Proyectos
        // -----------------------------------------------------------------
        [HttpGet]
        public async Task<IActionResult> GetProyectos()
        {
            var proyectos = await _context.Proyectos
                .Include(p => p.Estimaciones)
                .ToListAsync();
            return Ok(proyectos);
        }

        // -----------------------------------------------------------------
        // POST: Crear Proyecto
        // -----------------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> PostProyecto([FromBody] Proyecto proyecto)
        {
            _context.Proyectos.Add(proyecto);
            await _context.SaveChangesAsync();
            
            return CreatedAtAction(nameof(GetProyecto), new { id = proyecto.ProyectoID }, proyecto);
        }

        // -----------------------------------------------------------------
        // PUT: Actualizar Proyecto Completo
        // -----------------------------------------------------------------
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProyecto(int id, [FromBody] Proyecto proyecto)
        {
            if (id != proyecto.ProyectoID)
            {
                return BadRequest("El ID de la ruta no coincide con el ID del cuerpo.");
            }

            var proyectoExistente = await _context.Proyectos.FindAsync(id);
            if (proyectoExistente == null)
            {
                return NotFound($"Proyecto con ID {id} no encontrado.");
            }

            proyectoExistente.NombreObra = proyecto.NombreObra;
            proyectoExistente.Ubicacion = proyecto.Ubicacion;
            proyectoExistente.FechaInicio = proyecto.FechaInicio;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(409, "Conflicto de concurrencia al actualizar el proyecto.");
            }

            return NoContent();
        }

        // -----------------------------------------------------------------
        // PATCH: Actualizar Proyecto Parcial
        // -----------------------------------------------------------------
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchProyecto(int id, [FromBody] ProyectoPatchDTO patchDto)
        {
            var proyecto = await _context.Proyectos.FindAsync(id);
            if (proyecto == null)
            {
                return NotFound($"Proyecto con ID {id} no encontrado.");
            }

            if (patchDto.NombreObra != null)
            {
                proyecto.NombreObra = patchDto.NombreObra;
            }

            if (patchDto.Ubicacion != null)
            {
                proyecto.Ubicacion = patchDto.Ubicacion;
            }

            if (patchDto.FechaInicio.HasValue)
            {
                proyecto.FechaInicio = patchDto.FechaInicio.Value;
            }

            await _context.SaveChangesAsync();

            return Ok(proyecto);
        }

        // -----------------------------------------------------------------
        // DELETE: Eliminar Proyecto (CON VALIDACION MEJORADA)
        // -----------------------------------------------------------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProyecto(int id)
        {
            var proyecto = await _context.Proyectos
                .Include(p => p.Estimaciones)
                .FirstOrDefaultAsync(p => p.ProyectoID == id);

            if (proyecto == null)
            {
                return NotFound($"Proyecto con ID {id} no encontrado.");
            }

            // Validar que no tenga estimaciones
            if (proyecto.Estimaciones != null && proyecto.Estimaciones.Any())
            {
                return BadRequest(new 
                { 
                    error = "No se puede eliminar el proyecto",
                    razon = "El proyecto tiene estimaciones de costo asociadas. Elimínelas primero.",
                    estimacionesCount = proyecto.Estimaciones.Count
                });
            }
            
            _context.Proyectos.Remove(proyecto);
            await _context.SaveChangesAsync();
            
            return NoContent();
        }

        // -----------------------------------------------------------------
        // ENDPOINT DIFERENCIADOR: Analisis de Desviacion Presupuestal
        // Logica Algoritmica (Formula EAC)
        // -----------------------------------------------------------------
        [HttpGet("Desviacion/{proyectoId}")]
        public async Task<IActionResult> GetDesviacionFinanciera(int proyectoId)
        {
            var estimacionesConAvances = await _context.EstimacionesCosto
                .Include(e => e.Avances)
                .Where(e => e.ProyectoID == proyectoId)
                .ToListAsync();

            if (!estimacionesConAvances.Any())
            {
                return NotFound($"Proyecto con ID {proyectoId} o sus estimaciones no encontradas.");
            }

            decimal presupuestoTotalEstimado = estimacionesConAvances.Sum(e => e.MontoEstimado);
            decimal costoEjecutadoTotal = estimacionesConAvances.SelectMany(e => e.Avances).Sum(a => a.MontoEjecutado);
            
            decimal totalAvance = estimacionesConAvances.Average(e => e.Avances.Any() ? e.Avances.Average(a => a.PorcentajeCompletado) : 0m);
            decimal avanceFisicoDecimal = totalAvance / 100.00m; 

            if (avanceFisicoDecimal <= 0)
            {
                return Ok(new 
                { 
                    Riesgo = "SIN AVANCE", 
                    Mensaje = "No se puede proyectar el costo final sin avance registrado.", 
                    CostoEstimado = presupuestoTotalEstimado 
                });
            }

            decimal costoProyectadoFinal = costoEjecutadoTotal / avanceFisicoDecimal;
            decimal desviacionPorcentaje = (costoProyectadoFinal - presupuestoTotalEstimado) / presupuestoTotalEstimado * 100;
            string riesgo = desviacionPorcentaje > 5.0m ? "ALTO" : (desviacionPorcentaje > 0 ? "MEDIO" : "BAJO");

            return Ok(new
            {
                RiesgoDesviacion = riesgo,
                DesviacionPorcentaje = Math.Round(desviacionPorcentaje, 2),
                CostoEstimado = presupuestoTotalEstimado,
                CostoProyectadoFinal = Math.Round(costoProyectadoFinal, 2),
                Mensaje = $"El proyecto tiene un avance físico promedio del {Math.Round(totalAvance, 2)}%."
            });
        }
    }
}
