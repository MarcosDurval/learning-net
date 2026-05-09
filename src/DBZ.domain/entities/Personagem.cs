namespace DBZ.Domain.Entities;

public class Personagem
{
    public Guid? Id { get; set; }

    public required string Name { get; set; }

    public int Power { get; set; }

    // Raça do personagem (ex.: Saiyan, Humano). Pode influenciar a descrição.
    public string? Race { get; set; }

    public string? Transformation { get; set; }

    // Método de domínio que produz a descrição/greeting do personagem.
    // Torne virtual para que subclasses especializadas possam sobrescrever.
    public virtual string Describe()
    {
        return $"oi eu sou o {Name} tenho um poder de {Power}";
    }

    public string? Description { get; set; }

}
