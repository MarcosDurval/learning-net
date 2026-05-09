using Microsoft.AspNetCore.Mvc;
using DBZ.Application.Services;
using DBZ.Domain.Entities;
using DBZ.Application.DTOs;
using DBZ.Presentation.DTOs;

namespace DBZ.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonagensController : ControllerBase
{
    private readonly IPersonagemService _personagemService;

    public PersonagensController(IPersonagemService personagemService)
    {
        _personagemService = personagemService;
    }

    [HttpPost]
    public async Task<IActionResult> AddPersonagem([FromBody] CreatePersonagemDTO dto)
    {
        try
        {
            var createdPersonagem = await _personagemService.AddAsync(new Personagem
            {
                Name = dto.Name,
                Power = dto.Power,
                Race = dto.Race,
                Description = dto.Description
            });

            var result = await _personagemService.GetByIdAsync(createdPersonagem.Id!.Value);
            return CreatedAtAction(nameof(GetPersonagemById), new { id = createdPersonagem.Id }, result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPersonagemById(Guid id)
    {
        PersonagemResult? personagem = await _personagemService.GetByIdAsync(id);
        if (personagem is null)
        {
            return NotFound();
        }

        return Ok(personagem);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPersonagens()
    {
        IEnumerable<Personagem> personagens = await _personagemService.GetAllAsync();
        return Ok(personagens);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePersonagem([FromBody] EditPersonagemDTO dto, Guid id)
    {
        Personagem? updatedPersonagem;

        try
        {
            updatedPersonagem = await _personagemService.UpdateAsync(id, new Personagem
            {
                Name = dto.Name,
                Power = dto.Power,
                Race = dto.Race,
                Description = dto.Description
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }

        if (updatedPersonagem is null)
        {
            return NotFound();
        }

        return Ok(updatedPersonagem);
    }

    [HttpPost("{id}/transformations/{transformation}")]
    public async Task<IActionResult> TransformPersonagem(Guid id, string transformation)
    {
        return await TransformPersonagem(id, new TransformPersonagemDTO { Transformation = transformation });
    }

    [HttpPost("{id}/transformations")]
    public async Task<IActionResult> TransformPersonagem(Guid id, [FromBody] TransformPersonagemDTO dto)
    {
        var personagem = await _personagemService.GetEntityByIdAsync(id);

        if (personagem is null)
        {
            return NotFound();
        }

        var transformedPersonagem = await _personagemService.TransformAsync(id, dto.Transformation);

        if (transformedPersonagem is null)
        {
            var availableTransformations = _personagemService.GetAvailableTransformations(personagem);
            var availableText = availableTransformations.Count == 0
                ? "no transformations"
                : string.Join(", ", availableTransformations);

            return BadRequest($"{personagem.Race ?? "Race"} cannot transform into {dto.Transformation}. Available transformations: {availableText}.");
        }

        return Ok(transformedPersonagem);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePersonagem(Guid id)
    {
        await _personagemService.DeleteAsync(id);
        return NoContent();
    }
}
