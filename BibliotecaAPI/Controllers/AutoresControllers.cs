using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using BibliotecaAPI.Entidades;
using BibliotecaAPI.Datos;
using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaAPI.Controllers
{

    [ApiController]
    [Route("api/autores")] //  ruta donde es enviada la peticion http 
    public class AutoresControllers : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public AutoresControllers(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<Autor>> Get()
        {
            {
                return await context.Autores.ToListAsync();
            }; 
        }

        [HttpGet("{id:int}")] // api/autores/id
        public async Task<ActionResult<Autor>> Get(int id)
            {
            var autor = await context.Autores
                .Include(x => x.Libros)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (autor is null)
            {
                return NotFound();
            }

            return autor;
        }

        [HttpPost]
        public async Task<ActionResult> Post(Autor autor)
        {
            context.Add(autor);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")] // api/autores/id
        public async Task<ActionResult> Put(int id, Autor autor)
        {
            if (id != autor.Id)
            {
                return BadRequest("Los ids deben de coincidir");
            }

            context.Update(autor);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var registroBorrados = await context.Autores.Where(x => x.Id == id).ExecuteDeleteAsync();

            if (registroBorrados == 0)
            {
                return NotFound();
            }
            return Ok();
        }
        
    }
}
