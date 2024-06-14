using System.Transactions;
using Kolokwium2.Context;
using Kolokwium2.Models;
using Microsoft.EntityFrameworkCore;

namespace Kolokwium2.Service;

public class DbService : IDBService
{

    private CharacterContext Context;

    public DbService(CharacterContext context)
    {
        Context = context;
    }

    public async Task<bool> DoesCharacterExist(int characterId)
    {
        return await Context.Characters.FindAsync(characterId) != null;
    }

    public async Task<bool> DoesItemExist(int idItem)
    {
        return await Context.Items.FindAsync(idItem) != null;
    }

    public async Task<bool> DoesCharacterHaveCapacity(int characterId, List<int> idItems)
    {
        int ilosc = 0;
        foreach (var el in idItems)
        {
            Item item = await Context.Items.FindAsync(el);
            if (item == null)
            {
                throw new Exception("cos poszlo nie tak z itemem");
            }

            ilosc += item.Weight;
        }

        Character postac = await Context.Characters.FindAsync(characterId);

        if (postac == null)
        {
            throw new Exception("cos poszlo nie tak z postacia");
        }

        return (postac.CurrentWeight += ilosc) <= postac.MaxWeight;

    }

    public async Task<object> GetCharacters(int characterId)
    {
        var postac = await Context.Characters
            .Include(x => x.CharacterTitles)
            .ThenInclude(x => x.TitleNavigation)
            .Include(x => x.Backpacks)
            .ThenInclude(x => x.ItemNavigation)
            .FirstOrDefaultAsync(x => x.Id == characterId);

        if (postac == null)
        {
            throw new Exception("cos poszlo nie tak z postacia");
        }

        var odp = new
        {
            postac.FirstName,
            postac.LastName,
            postac.CurrentWeight,
            postac.MaxWeight,
            BackPackItems = postac.Backpacks.Select(x => new
            {
                x.ItemNavigation.Name,
                x.ItemNavigation.Weight,
                x.Amount
            }),
            Titles = postac.CharacterTitles.Select(x => new
            {
                Title = x.TitleNavigation.Name,
                x.AcquiredAt
            })
        };

        return odp;
    }

    public async Task<List<object>> GetCharactersItems(int characterId)
    {
        var odpowiedz = await Context.Backpacks.Where(x => x.CharacterId == characterId)
            .Select(x => new
            {
                x.Amount,
                x.ItemId,
                characterId
            }).ToListAsync();
        return odpowiedz.Cast<object>().ToList();
    }

    public async Task AddItemToBackpack(int characterId, List<int> idItems)
    {
        using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            foreach (var el in idItems)
            {
                Backpack? backpack =
                    await Context.Backpacks.FirstOrDefaultAsync(x =>
                        x.CharacterId == characterId && x.ItemId == el);

                if (backpack == null)
                {
                    Context.Backpacks.Add(new Backpack()
                    {
                        ItemId = el,
                        CharacterId = characterId,
                        Amount = 1
                    });
                }
                else
                {
                    backpack.Amount += 1;
                }

                await Context.SaveChangesAsync();
            }

            int ilosc = 0;
            foreach (var el in idItems)
            {
                Item item = await Context.Items.FindAsync(el);

                if (item == null)
                {
                    throw new Exception("cos poszlo nie tak z itemem");
                }

                ilosc += item.Weight;
            }


            Character postac = await Context.Characters.FindAsync(characterId);

            if (postac == null)
            {
                throw new Exception("cos poszlo nie tak z postacia");
            }

            postac.CurrentWeight += ilosc;
            await Context.SaveChangesAsync();

            scope.Complete();
        }
    }
}