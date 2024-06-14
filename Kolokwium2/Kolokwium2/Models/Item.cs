using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kolokwium2.Models;

[Table("items")]
public class Item
{
    [Key]
    public int Id { get; set; }

    [Required] [MaxLength(100)] public string Name { get; set; } = null!;
    
    [Required]
    public int Weight { get; set; }

    public List<Backpack> Backpacks { get; set; } = new List<Backpack>();
}