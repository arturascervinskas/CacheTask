namespace Domain.Entities;

public class ItemEntity : BaseEntity
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Address { get; set; }
}
