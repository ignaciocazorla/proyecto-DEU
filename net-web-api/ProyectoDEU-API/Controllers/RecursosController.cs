﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoDEU_API;
using ProyectoDEU_API.Models.DTO;

namespace ProyectoDEU_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecursosController : ControllerBase
    {
        private readonly ProyectoDEUContext _context;

        public RecursosController(ProyectoDEUContext context)
        {
            _context = context;
        }

        // GET: api/Recursos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Recurso>>> GetRecursos()
        {
          if (_context.Recursos == null)
          {
              return NotFound();
          }
            return await _context.Recursos.ToListAsync();
        }

        // GET: api/Recursos/5
        [HttpGet("{id}")]
        public IActionResult GetRecurso(Guid id)
        {
          if (_context.Recursos == null)
          {
              return NotFound();
          }
            var result = _context.Recursos
                .Include(e => e.Enlaces)
                .Where(e => e.Id == id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("curso/{idCurso}")]
        public IActionResult GetRecursosPorCurso(Guid idCurso)
        {
            if (idCurso == Guid.Empty)
            {
                return BadRequest();
            }
            var result = _context.Recursos
                .Include(e => e.Enlaces)
                .Where(r => r.IdCurso == idCurso)
                .AsQueryable();

            return Ok(result);
        }

        // PUT: api/Recursos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRecurso(Guid id, RecursoModificadoDTO recurso)
        {
            //if (id != recurso.Id)
            //{
            //    return BadRequest();
            //}
            var recursoDB = await _context.Recursos.FindAsync(id);
            if (recursoDB == null)
                return BadRequest();

            recursoDB.Titulo = recurso.Titulo;
            recursoDB.Texto = recurso.Texto;
            // agrego enlaces nuevos
            foreach (var enlace in recurso.Enlaces)
            {
                var e = new EnlaceRecurso
                {
                    Id = Guid.NewGuid(),
                    IdRecurso = id,
                    Url = enlace.Url,
                    Nombre = enlace.Nombre
                };
                recursoDB.Enlaces.Add(e);
            }

            // borro enlaces
            foreach (var idBaja in recurso.EnlacesBaja)
            {
                var e = await _context.EnlaceRecursos.FindAsync(idBaja);
                if (e != null)
                    _context.Entry(e).State = EntityState.Deleted;

                //recursoDB.Enlaces.Remove(e);
            }

            _context.Entry(recursoDB).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecursoExists(id))
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

        // POST: api/Recursos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Recurso>> PostRecurso(RecursoDTO recursoDTO)
        {
            if (_context.Recursos == null)
            {
                return Problem("Entity set 'ProyectoDEUContext.Recursos'  is null.");
            }
            var recurso = new Recurso
            {
                Id = Guid.NewGuid(),
                Titulo = recursoDTO.Titulo,
                Texto = recursoDTO.Texto,
                IdCurso = recursoDTO.IdCurso
            };

            foreach (var e in recursoDTO.Enlaces)
            {
                var enlace = new EnlaceRecurso
                {
                    Id = Guid.NewGuid(),
                    IdRecurso = recurso.Id,
                    Url = e.Url,
                    Nombre = e.Nombre
                };
                recurso.Enlaces.Add(enlace);
            };

            //recurso.Id = Guid.NewGuid();
            _context.Recursos.Add(recurso);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (RecursoExists(recurso.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetRecurso", new { id = recurso.Id }, recurso);
        }

        // DELETE: api/Recursos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecurso(Guid id)
        {
            if (_context.Recursos == null)
            {
                return NotFound();
            }
            var recurso = await _context.Recursos
                .FindAsync(id);
            if (recurso == null)
            {
                return NotFound();
            }

            //var enlaces = _context.EnlaceRecursos.Where(e => e.IdRecurso == id).AsQueryable();
            //foreach (var enlace in enlaces)
            //{
            //    _context.EnlaceRecursos.Remove(enlace);
            //}

            VaciarRecurso(id);

            _context.Recursos.Remove(recurso);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private void VaciarRecurso(Guid id)
        {
            var enlaces = _context.EnlaceRecursos.Where(e => e.IdRecurso == id).AsQueryable();
            foreach (var enlace in enlaces)
            {
                _context.EnlaceRecursos.Remove(enlace);
            }
        }

        private bool RecursoExists(Guid id)
        {
            return (_context.Recursos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
