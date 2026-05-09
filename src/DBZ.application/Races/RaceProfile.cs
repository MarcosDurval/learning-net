namespace DBZ.Application.Races;

public sealed class RaceProfile
{
    public required string Race { get; init; }

    public required string DescriptionTemplate { get; init; }

    public int PowerBonus { get; init; }

    public IReadOnlyDictionary<string, TransformationProfile> Transformations { get; init; }
        = new Dictionary<string, TransformationProfile>(StringComparer.OrdinalIgnoreCase);
}
