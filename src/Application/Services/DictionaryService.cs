using Application.Dto;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Application.Services;

public class DictionaryService
{
    private readonly IDictionaryRepository _userRepository;

    public DictionaryService(IDictionaryRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Dictionary> Get(Guid id)
    {
        DictionaryEntity userEntity = await _userRepository.Get(id) ?? throw new NotFoundException("User not found in DB");

        Dictionary user = new()
        {
            Id = id,
            Name = userEntity.Name,
            Address = userEntity.Address,
        };

        return user;
    }

    public async Task<List<Dictionary>> Get()
    {
        List<Dictionary> users = [];
        IEnumerable<DictionaryEntity> usersEntities = await _userRepository.Get();

        if (!usersEntities.Any())
            return [];

        users = usersEntities.Select(o => new Dictionary()
        {
            Id = o.Id,
            Name = o.Name,
            Address = o.Address,
        }).ToList();

        return users;
    }

    public async Task<Guid> Add(UserAdd user)
    {
        DictionaryEntity userEntity = new()
        {
            Name = user.Name,
            Address = user.Address,
        };

        return await _userRepository.Add(userEntity);
    }

    public async Task Update(Guid id, UserAdd user)
    {
        await Get(id);

        DictionaryEntity itemEntity = new()
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
