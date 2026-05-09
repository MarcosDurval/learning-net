using System.ComponentModel.DataAnnotations;

namespace DBZ.Presentation.DTOs;

public class TransformPersonagemDTO
{
    [Required(ErrorMessage = "Transformacao é obrigatoria")]
    public required string Transformation { get; set; }
}
