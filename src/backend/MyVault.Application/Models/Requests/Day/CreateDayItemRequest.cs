using System;

namespace MyVault.Application.Models.Requests.Day;

public class CreateDayItemRequest
{
    public required int DayId { get; set; }
    public required int Identifier { get; set; }
    public required int Type { get; set; }
    public required string Note { get; set; }
    public int? SubType { get; set; }
    public string? Time { get; set; }
}
