using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Kolokwium2.Models;

[Table("character_titles")]
[PrimaryKey(nameof(CharacterId), nameof(TitleId))]
public class CharacterTitle
{
    public int CharacterId { get; set; }

    public int TitleId { get; set; }

    [Required]
    public DateTime AcquiredAt { get; set; }

    [ForeignKey(nameof(TitleId))] public Title TitleNavigation { get; set; } = null!;
    
    [ForeignKey(nameof(CharacterId))] public Character CharacterNavigation { get; set; } = null!;

}