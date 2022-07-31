using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoDEU_API;

namespace ProyectoDEU_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CursosController : ControllerBase
    {
        private readonly ProyectoDEUContext _context;

        public CursosController(ProyectoDEUContext context)
        {
            _context = context;
        }

        // GET: api/Cursos
        [HttpGet]
        public IActionResult GetCursos()
        {
            if (_context.Cursos == null)
            {
                return NotFound();
            }
            var result = _context.Cursos
                .Include(e => e.Estudiantes)
                .Include(e => e.Docente)
                .AsQueryable();

            return Ok(result.OrderBy(e => e.Nombre).AsQueryable());
        }

        // GET: api/Cursos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Curso>> GetCurso(Guid id)
        {
          if (_context.Cursos == null)
          {
              return NotFound();
          }
            var curso = await _context.Cursos.FindAsync(id);

            if (curso == null)
            {
                return NotFound();
            }

            return curso;
        }

        [HttpGet("docente/{id}")]
        public IActionResult GetCursosPorDocente(Guid docenteId)
        {
            if (docenteId == null)
            {
                return BadRequest();
            }
            var result = _context.Cursos.Where(e => e.IdDocente == docenteId).AsQueryable();
            if (result.Count() == 0)
                return NotFound();
            return Ok(result.OrderBy(e => e.Nombre).AsQueryable());
        }

        // PUT: api/Cursos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCurso(Guid id, Curso curso)
        {
            if (id != curso.Id)
            {
                return BadRequest();
            }

            _context.Entry(curso).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CursoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Cursos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Curso>> PostCurso(Curso curso)
        {
          if (_context.Cursos == null)
          {
              return Problem("Entity set 'ProyectoDEUContext.Cursos'  is null.");
          }
            curso.Id = Guid.NewGuid();
            _context.Cursos.Add(curso);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CursoExists(curso.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetCurso", new { id = curso.Id }, curso);
        }

        // DELETE: api/Cursos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCurso(Guid id)
        {
            if (_context.Cursos == null)
            {
                return NotFound();
            }
            var curso = await _context.Cursos.FindAsync(id);
            if (curso == null)
            {
                return NotFound();
            }

            _context.Cursos.Remove(curso);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Cursos/inscribir
        [HttpPost("inscribir")]
        public async Task<IActionResult> InscribirEstudiante([FromBody] InscripcionCurso inscripcion)
        {
            if (!CursoExists(inscripcion.CursoId))
            {
                return BadRequest();
            }
            var curso = await _context.Cursos.FindAsync(inscripcion.CursoId);
            var estudiante = await _context.Estudiantes.FindAsync(inscripcion.EstudianteId);

            curso.Estudiantes.Add(estudiante);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool CursoExists(Guid id)
        {
            return (_context.Cursos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
