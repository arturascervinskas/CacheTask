namespace Domain.Entities;

public class UserEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public byte[] Password { get; set; }
    public int Role { get; set; }
}
