namespace Application.Dto;

public class ItemDto
{
    public string Key { get; set; }
    public List<string> Value { get; set; }
    public int ExpirationPeriod { get; set; }
    public DateTime ExpirationDate { get; set; }
}
