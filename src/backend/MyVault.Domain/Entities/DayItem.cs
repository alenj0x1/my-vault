using MyVault.Domain.Enums;

namespace MyVault.Domain.Entities;

public class DayItem
{
    public int Id { get; set; } = 0;
    public required int DayId { get; set; }
    public required int Identifier { get; set; }
    public required string Time { get; set; }
    public required int Type { get; set; }
    public int? SubType { get; set; }
    public required string Note { get; set; } = "without a note.";
}
