using Domain.Entities;

namespace Domain.Interfaces;

public interface IItemRepository
{
    public Task<ItemEntity?> Get(Guid id);
    public Task<IEnumerable<ItemEntity>> Get();
    public Task<Guid> Add(ItemEntity user);
    public Task<int> Update(ItemEntity user);
    public Task Delete(Guid id);
}