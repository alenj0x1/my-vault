namespace MyVault.Domain.Entities;

public class Day
{
    public required int Id { get; set; }
    public required DateTime? Date { get; set; }
    public List<DayItem> Items { get; set; } = [];
}
