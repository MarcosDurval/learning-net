using DBZ.Domain.Entities;
using DBZ.Application.DTOs;

namespace DBZ.Application.Services;

public interface IPersonagemService
{
    Task<Personagem> AddAsync(Personagem personagem);

    Task<PersonagemResult?> GetByIdAsync(Guid id);

    Task<IEnumerable<Personagem>> GetAllAsync();

    Task<Personagem?> UpdateAsync(Guid id, Personagem personagem);

    Task<Personagem?> GetEntityByIdAsync(Guid id);

    IReadOnlyCollection<string> GetAvailableTransformations(Personagem personagem);

    Task<PersonagemResult?> TransformAsync(Guid id, string transformation);

    Task<bool> DeleteAsync(Guid id);
}
