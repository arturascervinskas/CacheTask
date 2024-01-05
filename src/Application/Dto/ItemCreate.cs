namespace Application.Dto;

public class ItemCreate
{
    public string Key { get; set; }
    public List<object> Value { get; set; }
    public int? ExpirationPeriod { get; set; }
}
