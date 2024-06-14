namespace Kolokwium2.Service;

public interface IDBService
{
    Task<bool> DoesCharacterExist(int characterId);
    Task<object> GetCharacters(int characterId);
    Task<bool> DoesItemExist(int idItem);
    Task<bool> DoesCharacterHaveCapacity(int characterId, List<int> idItems);
    Task AddItemToBackpack(int characterId, List<int> idItems);
    Task<List<object>> GetCharactersItems(int characterId);
}