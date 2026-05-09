using DBZ.Application.Abstractions;
using DBZ.Domain.Entities;

namespace DBZ.Application.Races;

public sealed class RaceStrategy : IRaceStrategy
{
    private readonly IRaceProfileProvider _profiles;

    public RaceStrategy(IRaceProfileProvider profiles)
    {
        _profiles = profiles;
    }

    public string Describe(Personagem personagem)
    {
        var power = CalculatePower(personagem);
        var profile = _profiles.GetProfile(personagem.Race);

        return profile.DescriptionTemplate
            .Replace("{Name}", personagem.Name)
            .Replace("{Power}", power.ToString());
    }

    public int CalculatePower(Personagem personagem)
    {
        var profile = _profiles.GetProfile(personagem.Race);
        var power = personagem.Power + profile.PowerBonus;

        if (!string.IsNullOrWhiteSpace(personagem.Transformation)
            && profile.Transformations.TryGetValue(personagem.Transformation, out var transformation))
        {
            power += transformation.PowerBonus;
        }

        return power;
    }

    public bool IsKnownRace(string? race)
    {
        return _profiles.HasProfile(race);
    }

    public IReadOnlyCollection<string> GetKnownRaces()
    {
        return _profiles.GetProfiles()
            .Select(profile => profile.Race)
            .ToArray();
    }

    public IReadOnlyCollection<string> GetAvailableTransformations(Personagem personagem)
    {
        var profile = _profiles.GetProfile(personagem.Race);
        return profile.Transformations.Keys.ToArray();
    }

    public bool CanTransform(Personagem personagem, string transformation)
    {
        if (string.IsNullOrWhiteSpace(transformation))
        {
            return false;
        }

        var profile = _profiles.GetProfile(personagem.Race);
        return profile.Transformations.ContainsKey(transformation);
    }

    public void Transform(Personagem personagem, string transformation)
    {
        if (!CanTransform(personagem, transformation))
        {
            throw new InvalidOperationException($"{personagem.Race ?? "Race"} cannot transform into {transformation}.");
        }

        var profile = _profiles.GetProfile(personagem.Race);
        personagem.Transformation = profile.Transformations[transformation].Name;
    }
}
