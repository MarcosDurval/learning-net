using DBZ.Application.Races;

namespace DBZ.Application.Abstractions;

public interface IRaceProfileProvider
{
    RaceProfile GetProfile(string? race);

    IReadOnlyCollection<RaceProfile> GetProfiles();

    bool HasProfile(string? race);
}
