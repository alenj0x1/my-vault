using System;
using MyVault.Domain.Enums;

namespace MyVault.Domain.Entities;

public class Day
{
    public required int Id { get; set; }
    public required DateTime? Date { get; set; }
    public List<DayItem> Items { get; set; } = [];
    public Dictionary<string, string> Errors { get; set; } = [];
}
