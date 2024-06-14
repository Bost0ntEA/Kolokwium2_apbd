using Kolokwium2.Service;
using Microsoft.AspNetCore.Mvc;

namespace Kolokwium2.Controller;

[ApiController]
public class CharacterController: ControllerBase
{
    private IDBService Service;
    public CharacterController(IDBService service)
    {
        Service = service;
    }

    [HttpGet]
    [Route("api/characters/{idCharacter:int}")]
    public async Task<IActionResult> GetCharacter(int idCharacter)
    {
        if (!await Service.DoesCharacterExist(idCharacter))
        {
            return NotFound($"Postac z id {idCharacter} nie istnieje");
        }

        var result = await Service.GetCharacters(idCharacter);

        return Ok(result);
    }
    
    [HttpPost]
    [Route("api/characters/{idCharacter:int}/backpacks")]
    public async Task<IActionResult> AddItems(int idCharacter, List<int> itemsList)
    {
        foreach (var el in itemsList)
        {
            if (!await Service.DoesItemExist(el))
            {
                return NotFound($"przedmiot z id {el} nie istnieje");
            }
        }

        if (!await Service.DoesCharacterHaveCapacity(idCharacter, itemsList))
        {
            return NotFound($"Postac z id {idCharacter} nie poniesie takiego ciezaru");
        }

        await Service.AddItemToBackpack(idCharacter, itemsList);

        var odp = await Service.GetCharactersItems(idCharacter);

        return Ok(odp);
    }
}