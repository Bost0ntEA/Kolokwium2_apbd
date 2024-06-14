using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Kolokwium2.Models;

namespace Kolokwium2.Models;

[Table("characters")]
public class Character
{
    [Key]
    public int Id { get; set; }

    [Required] [MaxLength(50)] public string FirstName { get; set; } = null!;
    
    [Required] [MaxLength(120)] public string LastName { get; set; } = null!;

    [Required]
    public int CurrentWeight { get; set; }
    
    [Required]
    public int MaxWeight { get; set; }
    
    public List<CharacterTitle> CharacterTitles { get; set; } = new List<CharacterTitle>();

    public List<Backpack> Backpacks { get; set; } = new List<Backpack>();

}