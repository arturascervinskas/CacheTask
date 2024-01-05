using Domain.Entities;

namespace Domain.Interfaces;

public interface IItemRepository
{
    public Task<IEnumerable<ItemEntity>> Get();
    public Task<ItemEntity?> Get(string key);
    public Task<ItemEntity?> Create(ItemEntity itemEntity);
    public Task<ItemEntity?> Update(ItemEntity itemEntity);
    public Task Delete(string key);
}