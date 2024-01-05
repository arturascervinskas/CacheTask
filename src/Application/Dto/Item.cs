namespace Application.Dto;

public class Item
{
    public string Key { get; set; }
    public List<string> Value { get; set; }
    public int ExpirationPeriod { get; set; }
    public DateTime ExpirationDate { get; set; }
}
