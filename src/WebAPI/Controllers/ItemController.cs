using Application.Dto;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;

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

    [HttpGet("byKey")]
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

    [HttpDelete("byKey")]
    public async Task<IActionResult> Delete(string key)
    {
        await _itemService.Delete(key);

        var response = new
        {
            Message = "Key deleted",
        };

        return Ok(response);
    }

}