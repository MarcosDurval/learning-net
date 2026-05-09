using System.ComponentModel.DataAnnotations;

namespace DBZ.Presentation.DTOs;

public class CreatePersonagemDTO
{
    [Required(ErrorMessage = "Nome é obrigatorio")]
    [MinLength(3, ErrorMessage = "O nome deve ter no minimo 3 caracteres")]
    public required string Name { get; set; }

    [Required(ErrorMessage = "Poder é obrigatorio")]
    [Range(0, int.MaxValue, ErrorMessage = "Poder deve ser maior ou igual a zero")]
    public int Power { get; set; }

    public string? Race { get; set; }

    public string? Description { get; set; }
}
