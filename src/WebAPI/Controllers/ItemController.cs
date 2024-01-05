using Application.Dto;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

/// <summary>
/// Controller for handling item-related operations.
/// </summary>
[ApiController]
[Route("v1/[controller]")]
public class ItemController : ControllerBase
{
    private readonly ItemService _itemService;

    /// <summary>
    /// Constructor for the ItemController class.
    /// </summary>
    /// <param name="itemService">The service handling item-related operations.</param>
    public ItemController(ItemService itemService)
    {
        _itemService = itemService;
    }

    /// <summary>
    /// Retrieves a list of items.
    /// </summary>
    /// <returns>A list of items.</returns>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await _itemService.Get());
    }

    /// <summary>
    /// Retrieves a specific item by its key.
    /// </summary>
    /// <param name="key">The key of the item to retrieve.</param>
    /// <returns>The item corresponding to the provided key.</returns>
    [HttpGet("{key}")]
    public async Task<IActionResult> Get(string key)
    {
        return Ok(await _itemService.Get(key));
    }

    /// <summary>
    /// Creates a new item based on the provided item information.
    /// </summary>
    /// <param name="itemDto">The item information used to create the new item.</param>
    /// <returns>The newly created item.</returns>
    [HttpPost]
    public async Task<IActionResult> Create(ItemCreate itemDto)
    {
        string response = await _itemService.Create(itemDto);

        return Ok(new { response });
    }


    /// <summary>
    /// Updates an existing item based on the provided item information.
    /// </summary>
    /// <param name="itemDto">The updated item information.</param>
    /// <returns>The updated item.</returns>
    [HttpPut]
    public async Task<IActionResult> Update(ItemCreate itemDto)
    {
        string response = await _itemService.Update(itemDto);
        return Ok(new { response });
    }

    /// <summary>
    /// Deletes an item based on the provided key.
    /// </summary>
    /// <param name="key">The key of the item to delete.</param>
    /// <returns>A response indicating the result of the deletion.</returns>
    [HttpDelete("{key}")]
    public async Task<IActionResult> Delete(string key)
    {
        await _itemService.Delete(key);

        return Ok(new { response = "Key deleted"});
    }

}