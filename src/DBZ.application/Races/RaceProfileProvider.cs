using DBZ.Application.Abstractions;

namespace DBZ.Application.Races;

public sealed class RaceProfileProvider : IRaceProfileProvider
{
    private static readonly RaceProfile DefaultProfile = new()
    {
        Race = string.Empty,
        DescriptionTemplate = "oi eu sou o {Name} tenho um poder de {Power}",
        PowerBonus = 0
    };

    private static readonly IReadOnlyDictionary<string, RaceProfile> Profiles =
        new Dictionary<string, RaceProfile>(StringComparer.OrdinalIgnoreCase)
        {
            ["Saiyan"] = new RaceProfile
            {
                Race = "Saiyan",
                DescriptionTemplate = "Como um Saiyan, eu sou o {Name} e tenho um poder de {Power}",
                PowerBonus = 10,
                Transformations = new Dictionary<string, TransformationProfile>(StringComparer.OrdinalIgnoreCase)
                {
                    ["Super Saiyan"] = new TransformationProfile
                    {
                        Name = "Super Saiyan",
                        PowerBonus = 50
                    }
                }
            },
            ["Humano"] = new RaceProfile
            {
                Race = "Humano",
                DescriptionTemplate = "Como um Humano, eu sou o {Name} e tenho um poder de {Power}",
                PowerBonus = 2
            },
            ["Namekusei"] = new RaceProfile
            {
                Race = "Namekusei",
                DescriptionTemplate = "Como um Namekusei, eu sou o {Name} e tenho um poder de {Power}",
                PowerBonus = 8,
                Transformations = new Dictionary<string, TransformationProfile>(StringComparer.OrdinalIgnoreCase)
                {
                    ["Gigante"] = new TransformationProfile
                    {
                        Name = "Gigante",
                        PowerBonus = 35
                    }
                }
            }
        };

    public RaceProfile GetProfile(string? race)
    {
        if (string.IsNullOrWhiteSpace(race))
        {
            return DefaultProfile;
        }

        return Profiles.TryGetValue(race.Trim(), out var profile)
            ? profile
            : DefaultProfile;
    }

    public IReadOnlyCollection<RaceProfile> GetProfiles()
    {
        return Profiles.Values.ToArray();
    }

    public bool HasProfile(string? race)
    {
        return string.IsNullOrWhiteSpace(race) || Profiles.ContainsKey(race.Trim());
    }
}
