using System.ComponentModel.DataAnnotations;

namespace ProjectDBZ.Models
{
    public class Personagem
    {
        public Guid? Id { get; set; } = Guid.NewGuid();
        // [Required]
        public required string Name { get; set; }

        [Required]
        public required string Power { get; set; }

        public string? Description { get; set; }
    }
}