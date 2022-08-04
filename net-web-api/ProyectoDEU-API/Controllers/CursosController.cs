using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProyectoDEU_API;
using ProyectoDEU_API.Models.Authentication;

namespace ProyectoDEU_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CursosController : ControllerBase
    {
        private readonly ProyectoDEUContext _context;
        private readonly IConfiguration _configuration;


        public CursosController(ProyectoDEUContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
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
        public IActionResult GetCurso(Guid id)
        {
          if (_context.Cursos == null)
          {
              return NotFound();
          }
            var result = _context.Cursos
                .Include(e => e.Recursos)
                .Include("Recursos.Enlaces")
                .Where(e => e.Id == id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("usuario")]
        public IActionResult GetCursosPorUsuario()
        {

            //if (docenteId == null)
            //{
            //    return BadRequest();
            //}
            var request = HttpContext.Request;
            var token = default(string);

            if (request.Headers.ContainsKey("Authorization"))
            {
                if (AuthenticationHeaderValue.TryParse(request.Headers["Authorization"], out AuthenticationHeaderValue authorizationHeader))
                {
                    if (String.Equals(authorizationHeader.Scheme, "Bearer", StringComparison.InvariantCultureIgnoreCase))
                        token = authorizationHeader.Parameter;
                }
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenClaims = tokenHandler.ValidateToken(token,
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(_configuration.GetSection("JWT").GetValue<byte[]>("Secret")),
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

            var rol = tokenClaims.FindFirst(ClaimTypes.Role)?.Value;
            Guid userid = Guid.Parse(tokenClaims.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            IQueryable<Curso> result = new List<Curso>().AsQueryable();
            if (rol == UserRoles.Docente)
            {
                result = _context.Cursos
                        .Include(e => e.Recursos)
                        .Include("Recursos.Enlaces")
                        .Where(e => e.IdDocente == userid)
                        //.Include(c => c.Docente)
                        .AsQueryable();
            }
            if (rol == UserRoles.Estudiante)
            {
                Estudiante estudiante = _context.Estudiantes.FindAsync(userid).Result;
                result = _context.Cursos
                        .Include(e => e.Recursos)
                        .Include("Recursos.Enlaces")
                        .Where(e => e.Estudiantes.Contains(estudiante))
                        //.Include(c => c.Docente)
                        .AsQueryable();
            }

            
            //if (result.Count() == 0)
            //    return NotFound();
            return Ok(result);
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

            var curso = _context.Cursos
                .Include("Estudiantes")
                .Where(c => c.Id == id).First();
                //.FindAsync(id);
            if (curso == null)
            {
                return NotFound();
            }

            // primero borrar recursos del curso
            var recursos = _context.Recursos
                .Include(r => r.Enlaces)
                .Where(e => e.IdCurso == curso.Id)
                .AsQueryable();

            foreach (var recurso in recursos)
            {
                //vacio recurso
                foreach (var enlace in recurso.Enlaces)
                {
                    //_context.EnlaceRecursos.Remove(enlace);
                    _context.Entry(enlace).State = EntityState.Deleted;
                }

                //await _context.SaveChangesAsync();

                // borro recurso
                //_context.Entry(recurso).State = EntityState.Deleted;
                _context.Recursos.Remove(recurso);
            }

            _context.Cursos.Remove(curso);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Cursos/inscribir
        [HttpPost("inscribir")]
        public async Task<IActionResult> InscribirEstudiante([FromBody] InscripcionCursoDTO inscripcion)
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
