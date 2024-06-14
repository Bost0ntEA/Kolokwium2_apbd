using Kolokwium2.Models;
using Microsoft.EntityFrameworkCore;

namespace Kolokwium2.Context;

public class CharacterContext: DbContext
{
    public virtual DbSet<Backpack> Backpacks { get; set; }

    public virtual DbSet<Character> Characters { get; set; }

    public virtual DbSet<CharacterTitle> CharacterTitles { get; set; }

    public virtual DbSet<Item> Items { get; set; }
    
    public virtual DbSet<Title> Titles { get; set; }
    protected CharacterContext()
    { }
    public CharacterContext(DbContextOptions options) : base(options)
    { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.Entity<Title>().HasData(
            new Title(){Id = 1, Name = "Miszmaszer"},
            new Title(){Id = 2, Name = "Mixer"},
            new Title(){Id = 3, Name = "Mielonkowy Krol"}
        );
        
        modelBuilder.Entity<Character>().HasData(
            new Character(){Id = 1, FirstName = "Konorowicz", LastName = "Saladin", CurrentWeight = 10, MaxWeight = 120},
            new Character(){Id = 2, FirstName = "Samanta", LastName = "Mysz", CurrentWeight = 15, MaxWeight = 69},
            new Character(){Id = 3, FirstName = "Dzesika", LastName = "Brown", CurrentWeight = 10, MaxWeight = 200}
        );
        
        modelBuilder.Entity<Item>().HasData(
            new Item(){Id = 1, Name = "Tasak", Weight = 5},
            new Item(){Id = 2, Name = "Napiersnik", Weight = 10},
            new Item(){Id = 3, Name = "Widly", Weight = 15}
        );

        modelBuilder.Entity<Backpack>().HasData(
            new Backpack(){CharacterId = 1, ItemId = 1, Amount = 1},
            new Backpack(){CharacterId = 1, ItemId = 3, Amount = 1},
            new Backpack(){CharacterId = 2, ItemId = 2, Amount = 1}
        );
        
        modelBuilder.Entity<CharacterTitle>().HasData(
            new CharacterTitle(){CharacterId = 1, TitleId = 1, AcquiredAt = new DateTime(2015, 12, 24)},
            new CharacterTitle(){CharacterId = 3, TitleId = 2, AcquiredAt = new DateTime(2020, 5, 15)}
        );
        
        base.OnModelCreating(modelBuilder);
    }
}