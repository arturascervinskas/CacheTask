using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class ItemController : ControllerBase
{
    private readonly ItemService _dictionaryService;

    public ItemController(ItemService dictionaryService)
    {
        _dictionaryService = dictionaryService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await _dictionaryService.Get());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        return Ok(await _dictionaryService.Get(id));
    }
    [HttpPost]
    public async Task<IActionResult> Add(UserAdd item)
    {
        Guid guid = await _dictionaryService.Add(item);
        return CreatedAtAction(nameof(Get), new { Id = guid }, item);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UserAdd _item)
    {
        await _dictionaryService.Update(id, _item);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _dictionaryService.Delete(id);
        return Ok();
    }

}