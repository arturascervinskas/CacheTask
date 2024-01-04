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
        ItemEntity userEntity = await _userRepository.Get(id) ?? throw new NotFoundException("User not found in DB");

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

    //public async Task<Guid> Add(UserAdd user)
    //{
    //    ItemEntity userEntity = new()
    //    {
    //        Name = user.Name,
    //        Address = user.Address,
    //    };

    //    return await _userRepository.Add(userEntity);
    //}

    //public async Task Update(Guid id, UserAdd user)
    //{
    //    await Get(id);

    //    ItemEntity itemEntity = new()
    //    {
    //        Id = id,
    //        Name = user.Name,
    //        Address = user.Address,
    //    };

    //    int result = await _userRepository.Update(itemEntity);

    //    if (result > 1)
    //        throw new InvalidOperationException("Update was performed on multiple rows");
    //}

    public async Task Delete(Guid id)
    {
        await Get(id);

        await _userRepository.Delete(id);
    }
}
