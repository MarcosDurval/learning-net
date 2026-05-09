using DBZ.Domain.Entities;

namespace DBZ.Application.Abstractions;

public interface IRaceStrategy
{
    string Describe(Personagem personagem);

    int CalculatePower(Personagem personagem);

    bool IsKnownRace(string? race);

    IReadOnlyCollection<string> GetKnownRaces();

    IReadOnlyCollection<string> GetAvailableTransformations(Personagem personagem);

    bool CanTransform(Personagem personagem, string transformation);

    void Transform(Personagem personagem, string transformation);
}
