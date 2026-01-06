namespace MyVault.Domain.Entities;

public class Day
{
    public int Id { get; set; } = 0;
    public required DateTime? Date { get; set; }
    public List<DayItem> Items { get; set; } = [];
}
