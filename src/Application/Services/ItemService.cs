using Application.Dto;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace Application.Services;

public class ItemService
{
    private readonly IItemRepository _itemRepository;
    private readonly IConfiguration _configuration;
    private readonly int _maxItemExPeriod;
    private readonly int _defaultExPeriod;

    public ItemService(IItemRepository itemRepository, IConfiguration configuration)
    {
        _itemRepository = itemRepository;
        _configuration = configuration;
        _maxItemExPeriod = _configuration.GetValue<int>("MaxItemExpirationPeriod");
        _defaultExPeriod = _configuration.GetValue<int>("ItemExpirationPeriod");
    }

    public async Task<IEnumerable<Item>> Get()
    {
        IEnumerable<ItemEntity> itemEntities = await _itemRepository.Get();

        if (!itemEntities.Any())
            return [];

        IEnumerable<Item> items = itemEntities.Select(o => new Item
        {
            Key = o.Key,
            Value = JsonSerializer.Deserialize<List<object>>(o.Value),
            ExpirationPeriod = o.ExpirationPeriod,
            ExpirationDate = o.ExpirationDate
        });

        return items;
    }

    public async Task<Item> Get(string key)
    {
        ItemEntity itemEntity = await _itemRepository.Get(key) ?? throw new NotFoundException("Key not found");

        if (itemEntity.ExpirationDate < DateTime.UtcNow)
        {
            await _itemRepository.Delete(key);

            throw new NotFoundException("Key not found");
        }

        DateTime updatedDate = DateTime.UtcNow.AddSeconds(itemEntity.ExpirationPeriod);

        itemEntity.ExpirationDate = updatedDate;

        await _itemRepository.UpdateExDate(itemEntity);

        Item item = new()
        {
            Key = itemEntity.Key,
            Value = JsonSerializer.Deserialize<List<object>>(itemEntity.Value),
            ExpirationPeriod = itemEntity.ExpirationPeriod,
            ExpirationDate = itemEntity.ExpirationDate
        };

        return item;
    }

    public async Task<string> Create(ItemCreate itemDto)
    {
        if (itemDto.ExpirationPeriod <= 0)
            throw new ArgumentException("ExpirationPeriod cannot be negative.");

        int exPeriod = itemDto.ExpirationPeriod ?? _defaultExPeriod;

        if (exPeriod > _maxItemExPeriod)
            throw new ArgumentException($"Invalid ExpirationPeriod field");

        if (await _itemRepository.Get(itemDto.Key) is null)
        {
            ItemEntity itemEntity = new()
            {
                Key = itemDto.Key,
                Value = JsonSerializer.Serialize(itemDto.Value),
                ExpirationPeriod = exPeriod,
                ExpirationDate = DateTime.Now.AddSeconds(exPeriod)
            };

            await _itemRepository.Create(itemEntity);

            return "Item created";
        }
        else
        {
            ItemEntity itemEntity = new()
            {
                Key = itemDto.Key,
                Value = JsonSerializer.Serialize(itemDto.Value),
                ExpirationPeriod = exPeriod,
                ExpirationDate = DateTime.Now.AddSeconds(exPeriod)
            };

            await _itemRepository.Update(itemEntity);

            return "Item updated";
        }
    }

    public async Task<string> Update(ItemCreate itemDto)
    {
        if (itemDto.ExpirationPeriod <= 0)
            throw new ArgumentException("ExpirationPeriod cannot be negative.");

        int exPeriod = itemDto.ExpirationPeriod ?? _defaultExPeriod;

        if (exPeriod > _maxItemExPeriod)
            throw new ArgumentException($"Invalid ExpirationPeriod field");

        if (await _itemRepository.Get(itemDto.Key) is null)
        {
            ItemEntity itemEntity = new()
            {
                Key = itemDto.Key,
                Value = JsonSerializer.Serialize(itemDto.Value),
                ExpirationPeriod = exPeriod,
                ExpirationDate = DateTime.Now.AddSeconds(exPeriod)
            };

            await _itemRepository.Create(itemEntity);

            return "Item created";
        }
        else
        {
            ItemEntity itemEntity = new()
            {
                Key = itemDto.Key,
                Value = JsonSerializer.Serialize(itemDto.Value),
                ExpirationPeriod = exPeriod,
                ExpirationDate = DateTime.Now.AddSeconds(exPeriod)
            };

            await _itemRepository.Update(itemEntity);

            return "Item updated";
        }
    }

    public async Task Delete(string key)
    {
        await _itemRepository.Get(key);

        await _itemRepository.Delete(key);
    }
}
