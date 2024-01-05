using Application.Dto;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using System.Text.Json;

namespace Application.Services;

public class ItemService
{
    private readonly IItemRepository _itemRepository;

    public ItemService(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
    }

    public async Task<IEnumerable<ItemDto>> Get()
    {
        IEnumerable<ItemEntity> itemEntities = await _itemRepository.Get();

        if (!itemEntities.Any())
        {
            throw new NotFoundException("No items found");
        }

        IEnumerable<ItemDto> items = itemEntities.Select(itemEntity => new ItemDto
        {
            Key = itemEntity.Key,
            Value = JsonSerializer.Deserialize<List<string>>(itemEntity.Value),
            ExpirationPeriod = itemEntity.ExpirationPeriod
        });

        return items;
    }

    public async Task<ItemDto> Get(string key)
    {
        ItemEntity itemEntity = await _itemRepository.Get(key) ?? throw new NotFoundException("Key not found");

        ItemDto item = new()
        {
            Key = itemEntity.Key,
            Value = JsonSerializer.Deserialize<List<string>>(itemEntity.Value),
            ExpirationPeriod = itemEntity.ExpirationPeriod
        };

        return item;
    }

    public async Task<string> Create(ItemDto itemDto)
    {
        if (await _itemRepository.Get(itemDto.Key) is null)
        {
            ItemEntity itemEntity = new()
            {
                Key = itemDto.Key,
                Value = JsonSerializer.Serialize(itemDto.Value),
                ExpirationPeriod = itemDto.ExpirationPeriod,
                ExpirationDate = DateTime.Now.AddSeconds(itemDto.ExpirationPeriod)
            };

            await _itemRepository.Create(itemEntity);

            return "Item created";
        }
        else
        {
            await Update(itemDto);

            return "Item updated";
        }
    }

    public async Task<string> Update(ItemDto itemDto)
    {

        if (await _itemRepository.Get(itemDto.Key) is null)
        {
            await Create(itemDto);

            return "Item created";
        }
        else
        {
            ItemEntity itemEntity = new()
            {
                Key = itemDto.Key,
                Value = JsonSerializer.Serialize(itemDto.Value),
                ExpirationPeriod = itemDto.ExpirationPeriod
            };

            await _itemRepository.Update(itemEntity);

            return "Item updated";
        }
    }

    public async Task Delete(string key)
    {
        await Get(key);

        await _itemRepository.Delete(key);
    }
}
