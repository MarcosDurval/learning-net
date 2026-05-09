namespace DBZ.Application.DTOs;

public class RaceResult
{
    public string Race { get; set; } = string.Empty;

    public int PowerBonus { get; set; }

    public IReadOnlyCollection<string> Transformations { get; set; } = [];
}
