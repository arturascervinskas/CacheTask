using Application.Dto;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class ItemController : ControllerBase
{
    private readonly ItemService _itemService;

    public ItemController(ItemService itemService)
    {
        _itemService = itemService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await _itemService.Get());
    }

    [HttpGet("{key}")]
    public async Task<IActionResult> Get(string key)
    {
        return Ok(await _itemService.Get(key));
    }

    [HttpPost]
    public async Task<IActionResult> Create(ItemDto itemDto)
    {
        string response = await _itemService.Create(itemDto);

        return Ok(new { response });
    }

    [HttpPut]
    public async Task<IActionResult> Update(ItemDto itemDto)
    {
        string response = await _itemService.Update(itemDto);

        return Ok(new { response });
    }

    [HttpDelete("{key}")]
    public async Task<IActionResult> Delete(string key)
    {
        await _itemService.Delete(key);

        return Ok(new { response = "Key deleted"});
    }

}