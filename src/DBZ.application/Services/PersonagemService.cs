using DBZ.Application.Abstractions;
using DBZ.Domain.Entities;
using DBZ.Application.DTOs;

namespace DBZ.Application.Services;

public class PersonagemService : IPersonagemService
{
    private readonly IPersonagemRepository _personagemRepository;
    private readonly IRaceStrategy _raceStrategy;

    public PersonagemService(IPersonagemRepository personagemRepository, IRaceStrategy raceStrategy)
    {
        _personagemRepository = personagemRepository;
        _raceStrategy = raceStrategy;
    }

    public async Task<Personagem> AddAsync(Personagem personagem)
    {
        EnsureKnownRace(personagem.Race);

        personagem.Id ??= Guid.NewGuid();
        personagem.Race = NormalizeRace(personagem.Race);
        personagem.Transformation = null;

        await _personagemRepository.AddAsync(personagem);
        return personagem;
    }

    public async Task<PersonagemResult?> GetByIdAsync(Guid id)
    {
        Personagem? personagem = await _personagemRepository.GetByIdAsync(id);
        if (personagem is null) return null;

        return MapToResult(personagem);
    }

    public Task<IEnumerable<Personagem>> GetAllAsync()
    {
        return _personagemRepository.GetAllAsync();
    }

    public Task<Personagem?> GetEntityByIdAsync(Guid id)
    {
        return _personagemRepository.GetByIdAsync(id);
    }

    public IReadOnlyCollection<string> GetAvailableTransformations(Personagem personagem)
    {
        return _raceStrategy.GetAvailableTransformations(personagem);
    }

    public async Task<Personagem?> UpdateAsync(Guid id, Personagem personagem)
    {
        EnsureKnownRace(personagem.Race);

        var existingPersonagem = await _personagemRepository.GetByIdAsync(id);

        if (existingPersonagem is null)
        {
            return null;
        }

        existingPersonagem.Id = id;
        existingPersonagem.Name = personagem.Name;
        existingPersonagem.Power = personagem.Power;
        existingPersonagem.Race = NormalizeRace(personagem.Race);
        existingPersonagem.Transformation = null;
        existingPersonagem.Description = personagem.Description;

        await _personagemRepository.UpdateAsync(existingPersonagem);
        return existingPersonagem;
    }

    public async Task<PersonagemResult?> TransformAsync(Guid id, string transformation)
    {
        var personagem = await _personagemRepository.GetByIdAsync(id);

        if (personagem is null || !_raceStrategy.CanTransform(personagem, transformation))
        {
            return null;
        }

        _raceStrategy.Transform(personagem, transformation);
        await _personagemRepository.UpdateAsync(personagem);

        return MapToResult(personagem);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var personagem = await _personagemRepository.GetByIdAsync(id);

        if (personagem is null)
        {
            return false;
        }

        await _personagemRepository.DeleteAsync(personagem);
        return true;
    }

    private PersonagemResult MapToResult(Personagem personagem)
    {
        return new PersonagemResult
        {
            Id = personagem.Id,
            Name = personagem.Name,
            Power = personagem.Power,
            EffectivePower = _raceStrategy.CalculatePower(personagem),
            Race = personagem.Race,
            Transformation = personagem.Transformation,
            Description = personagem.Description,
            Greeting = _raceStrategy.Describe(personagem),
            AvailableTransformations = _raceStrategy.GetAvailableTransformations(personagem)
        };
    }

    private void EnsureKnownRace(string? race)
    {
        if (!_raceStrategy.IsKnownRace(race))
        {
            var availableRaces = string.Join(", ", _raceStrategy.GetKnownRaces());
            throw new ArgumentException($"Unknown race: {race}. Available races: {availableRaces}.", nameof(race));
        }
    }

    private static string? NormalizeRace(string? race)
    {
        return string.IsNullOrWhiteSpace(race) ? null : race.Trim();
    }
}
