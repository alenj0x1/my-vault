using System;
using MyVault.Domain.Enums;

namespace MyVault.Domain.Entities;

public class Day
{
    public required int Id { get; set; }
    public required DayIdentifier Identifier { get; set; }
    public required string Time { get; set; }
    public required DayType Type { get; set; }
    public DayType? SubType { get; set; }
    public required string Note { get; set; } = "without a note.";
}
