using System;

namespace MyVault.Application.Models.Requests.Day;

public class UpdateDayItemRequest
{
    public int? DayId { get; set; }
    public int? Identifier { get; set; }
    public int? Type { get; set; }
    public string? Note { get; set; }
    public int? SubType { get; set; }
    public string? Time { get; set; }
}
