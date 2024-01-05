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

    public async Task<IEnumerable<Item>> Get()
    {
        IEnumerable<ItemEntity> itemEntities = await _itemRepository.Get();

        if (!itemEntities.Any())
        {
            throw new NotFoundException("No items found");
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

        Item item = new()
        {
            Key = itemEntity.Key,
            Value = JsonSerializer.Deserialize<List<string>>(itemEntity.Value),
            ExpirationPeriod = itemEntity.ExpirationPeriod,
            ExpirationDate = itemEntity.ExpirationDate
        };

        return item;
    }

    public async Task<string> Create(ItemCreate itemDto)
    {
        if (await _itemRepository.Get(itemDto.Key) is null)
        {
            int ExPeriod = itemDto.ExpirationPeriod ?? 0;

            ItemEntity itemEntity = new()
            {
                Key = itemDto.Key,
                Value = JsonSerializer.Serialize(itemDto.Value),
                ExpirationPeriod = ExPeriod,
                ExpirationDate = DateTime.Now.AddSeconds(ExPeriod)
            };

            await _itemRepository.Create(itemEntity);

            return "Item created";
        }
        else
        {
            int ExPeriod = itemDto.ExpirationPeriod ?? 0;

            ItemEntity itemEntity = new()
            {
                Key = itemDto.Key,
                Value = JsonSerializer.Serialize(itemDto.Value),
                ExpirationPeriod = ExPeriod,
                ExpirationDate = DateTime.Now.AddSeconds(ExPeriod)
            };

            await _itemRepository.Create(itemEntity);

            return "Item created";
        }
    }

    public async Task<Guid> Add(UserAdd user)
    {
        ItemEntity userEntity = new()
        {
            Name = user.Name,
            Address = user.Address,
        };

        return await _userRepository.Add(userEntity);
    }

    public async Task Update(Guid id, UserAdd user)
    {
        await Get(id);

        ItemEntity itemEntity = new()
        {
            Id = id,
            Name = user.Name,
            Address = user.Address,
        };

        int result = await _userRepository.Update(itemEntity);

        if (result > 1)
            throw new InvalidOperationException("Update was performed on multiple rows");
    }

    public async Task Delete(Guid id)
    {
        await Get(id);

        await _userRepository.Delete(id);
    }
}
