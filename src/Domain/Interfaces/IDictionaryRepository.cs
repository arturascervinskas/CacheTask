using Domain.Entities;

namespace Domain.Interfaces;

public interface IDictionaryRepository
{
    public Task<DictionaryEntity?> Get(Guid id);
    public Task<IEnumerable<DictionaryEntity>> Get();
    public Task<Guid> Add(DictionaryEntity user);
    public Task<int> Update(DictionaryEntity user);
    public Task Delete(Guid id);
}