namespace DBZ.Domain.Entities;

public class Saiyan : Personagem
{
    // Poder adicional que um Saiyan pode ter (exemplo).
    public int PowerLevel { get; set; }

    public Saiyan()
    {
        Race = "Saiyan";
    }

    public override string Describe()
    {
        return $"Como um {Race}, oi eu sou o {Name} tenho um poder de {Power}";
    }
}
