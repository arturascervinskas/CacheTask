namespace Application.Dto;

public class Item
{
    public string Key { get; set; }
    public List<object> Value { get; set; }
    public int ExpirationPeriod { get; set; }
    public DateTime ExpirationDate { get; set; }
}
