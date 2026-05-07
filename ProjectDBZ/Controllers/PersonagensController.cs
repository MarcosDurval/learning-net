using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectDBZ.Data;
using ProjectDBZ.Models;

namespace ProjectDBZ.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonagensController : ControllerBase
    {
       private readonly AppDbContext _appDbContext;

       public PersonagensController(AppDbContext appDbContext)
        {
            // passamos o contexto para a variável local via injeção de dependência
            _appDbContext = appDbContext;
        }

        [HttpPost]
        public async Task<IActionResult> AddPersonagem([FromBody] Models.Personagem personagem)
        {
            if (personagem.Id is null || personagem.Id == Guid.Empty)
            {
                personagem.Id = Guid.NewGuid();
            }

            // Adiciona o personagem ao contexto e salva as mudanças no banco de dados
            _appDbContext.DBZ.Add(personagem);
            await _appDbContext.SaveChangesAsync();

            // Retorna uma resposta de sucesso com o personagem criado
            return CreatedAtAction(nameof(GetPersonagemById), new { id = personagem.Id }, personagem);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPersonagemById(Guid id)
        {
            var personagem = await _appDbContext.DBZ.FindAsync(id);
            if (personagem is null)
            {
                return NotFound();
            }

            return Ok(personagem);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPersonagens()
        {
            IEnumerable<Personagem> personagens = await _appDbContext.DBZ.ToListAsync();
            return Ok(personagens);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePersonagem([FromBody] Models.Personagem personagem, Guid id)
        {
            if (personagem.Id is not null && !Equals(personagem.Id, id))
            {
                return BadRequest("Route id does not match body id.");
            }

            var oldPersonagem = await _appDbContext.DBZ.FindAsync(id);

            if (oldPersonagem is null) return NotFound();
            _appDbContext.Entry(oldPersonagem).CurrentValues.SetValues(personagem);

            await _appDbContext.SaveChangesAsync();

            return Ok(personagem);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePersonagem(Guid id)
        {
            var personagem = await _appDbContext.DBZ.FindAsync(id);
            if (personagem is null)
            {
                return NoContent();
            }

            _appDbContext.DBZ.Remove(personagem);
            await _appDbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}