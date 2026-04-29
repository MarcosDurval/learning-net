using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectDBZ.Data;

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
            // _appDbContext.DBZ.Add(personagem);
            // if (!ModelState.IsValid)
            // {
            //     return BadRequest(ModelState);
            // }

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
            if (personagem == null)
            {
                return NotFound();
            }

            return Ok(personagem);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPersonagens()
        {
            var personagens = await _appDbContext.DBZ.FindAsync();
            return Ok(personagens);
        }
    }
}