using DBZ.Application.Abstractions;
using DBZ.Domain.Entities;
using DBZ.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DBZ.Infrastructure.Repositories;

public class PersonagemRepository : IPersonagemRepository
{
    private readonly AppDbContext _appDbContext;

    public PersonagemRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task AddAsync(Personagem personagem)
    {
        _appDbContext.DBZ.Add(personagem);
        await _appDbContext.SaveChangesAsync();
    }

    public Task<Personagem?> GetByIdAsync(Guid id)
    {
        return _appDbContext.DBZ.FindAsync(id).AsTask();
    }

    public async Task<IEnumerable<Personagem>> GetAllAsync()
    {
        return await _appDbContext.DBZ.ToListAsync();
    }

    public async Task UpdateAsync(Personagem personagem)
    {
        _appDbContext.DBZ.Update(personagem);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Personagem personagem)
    {
        _appDbContext.DBZ.Remove(personagem);
        await _appDbContext.SaveChangesAsync();
    }
}
