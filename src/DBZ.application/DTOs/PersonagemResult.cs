namespace DBZ.Application.DTOs;

public class PersonagemResult
{
    public Guid? Id { get; set; }

    public string Name { get; set; } = null!;

    public int Power { get; set; }

    public int EffectivePower { get; set; }

    public string? Race { get; set; }

    public string? Transformation { get; set; }

    public string? Description { get; set; }

    public string Greeting { get; set; } = string.Empty;

    public IReadOnlyCollection<string> AvailableTransformations { get; set; } = [];
}
