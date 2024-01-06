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
    public int MaxItemExPeriod { get; }
    public int DefaultExPeriod { get; }

    public ItemService(IItemRepository itemRepository, IConfiguration configuration)
    {
        _itemRepository = itemRepository;
        _configuration = configuration;
        MaxItemExPeriod = _configuration.GetValue<int>("MaxItemExpirationPeriod");
        DefaultExPeriod = _configuration.GetValue<int>("ItemExpirationPeriod");
    }

    public async Task<IEnumerable<Item>> Get()
    {
        IEnumerable<ItemEntity> itemEntities = await _itemRepository.Get();

        if (!itemEntities.Any())
        {
            return [];
        }

        IEnumerable<Item> items = itemEntities.Select(o => new Item
        {
            Key = o.Key,
            Value = JsonSerializer.Deserialize<List<string>>(o.Value),
            ExpirationPeriod = o.ExpirationPeriod,
            ExpirationDate = o.ExpirationDate
        });

        return items;
    }

    public async Task<Item> Get(string key)
    {
        ItemEntity itemEntity = await _itemRepository.Get(key) ?? throw new NotFoundException("Key not found");

        if (itemEntity.ExpirationDate > DateTime.UtcNow)
        {
            DateTime updatedDate = DateTime.UtcNow.AddSeconds(itemEntity.ExpirationPeriod);

            Item item = new()
            {
                Key = itemEntity.Key,
                Value = JsonSerializer.Deserialize<List<string>>(itemEntity.Value),
                ExpirationPeriod = itemEntity.ExpirationPeriod,
                ExpirationDate = updatedDate
            };

            await _itemRepository.UpdateExDate(itemEntity.Key, updatedDate);

            return item;
        }
        else
        {
            await _itemRepository.Delete(key);

            throw new NotFoundException();
        }

    }

    public async Task<string> Create(ItemCreate itemDto)
    {
        if (itemDto.ExpirationPeriod < MaxItemExPeriod)
        {
            if (await _itemRepository.Get(itemDto.Key) is null)
            {
                int exPeriod = itemDto.ExpirationPeriod is 0 ? DefaultExPeriod : itemDto.ExpirationPeriod.Value;

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

            throw new FoundException("Item with same key already exists.");
        }

        throw new ArgumentException($"Invalid ExpirationPeriod field");
    }

    public async Task<string> Update(ItemCreate itemDto)
    {
        if (itemDto.ExpirationPeriod < MaxItemExPeriod)
        {
            if (await _itemRepository.Get(itemDto.Key) is null)
            {
                int exPeriod = itemDto.ExpirationPeriod is 0 ? DefaultExPeriod : itemDto.ExpirationPeriod.Value;

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
                int exPeriod = itemDto.ExpirationPeriod is 0 ? DefaultExPeriod : itemDto.ExpirationPeriod.Value;

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

        throw new ArgumentException($"Invalid ExpirationPeriod field");
    }

    public async Task Delete(string key)
    {
        await Get(key);

        await _itemRepository.Delete(key);
    }
}
