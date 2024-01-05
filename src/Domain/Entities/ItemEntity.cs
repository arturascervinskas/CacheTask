namespace Domain.Entities;

public class ItemEntity
{
    public string Key { get; set; }
    public string Value { get; set; }
    public int ExpirationPeriod { get; set; }
    public DateTime ExpirationDate { get; set; }
}
