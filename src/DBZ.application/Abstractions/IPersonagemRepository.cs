using DBZ.Domain.Entities;

namespace DBZ.Application.Abstractions;

public interface IPersonagemRepository
{
    Task AddAsync(Personagem personagem);

    Task<Personagem?> GetByIdAsync(Guid id);

    Task<IEnumerable<Personagem>> GetAllAsync();

    Task UpdateAsync(Personagem personagem);

    Task DeleteAsync(Personagem personagem);
}
