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
            // Adiciona o personagem ao contexto e salva as mudanças no banco de dados
            _appDbContext.DBZ.Add(personagem);
            await _appDbContext.SaveChangesAsync();

            // Retorna uma resposta de sucesso com o personagem criado
            return CreatedAtAction(nameof(GetPersonagemById), new { id = personagem.Id }, personagem);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPersonagemById(int id)
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
        public async Task<IActionResult> UpdatePersonagem([FromBody] Models.Personagem personagem, int id)
        {
            var oldPersonagem = await _appDbContext.DBZ.FindAsync(id);

            if (oldPersonagem is null) return NotFound();
            _appDbContext.Entry(oldPersonagem).CurrentValues.SetValues(personagem);

            await _appDbContext.SaveChangesAsync();

            return Ok(personagem);
        }
    }
}