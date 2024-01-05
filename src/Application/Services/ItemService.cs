using Application.Dto;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Application.Services;

public class ItemService
{
    private readonly IItemRepository _userRepository;

    public ItemService(IItemRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Item> Get(Guid id)
    {
        ItemEntity userEntity = await _userRepository.Get(id) 
                                    ?? throw new NotFoundException("User not found in DB");

        Item user = new()
        {
            Id = id,
            Name = userEntity.Name,
            Address = userEntity.Address,
        };

        return user;
    }

    public async Task<List<Item>> Get()
    {
        List<Item> users = [];
        IEnumerable<ItemEntity> usersEntities = await _userRepository.Get();

        if (!usersEntities.Any())
            return [];

        users = usersEntities.Select(o => new Item()
        {
            Id = o.Id,
            Name = o.Name,
            Address = o.Address,
        }).ToList();

        return users;
    }
}
