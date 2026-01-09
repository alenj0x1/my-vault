using System;

namespace MyVault.Application.Models.Responses;

public class GenericResponse<T>
{
    public string Message { get; set; } = "completed";
    public bool Ok { get; set; }
    public T? Data { get; set; }
    public DateTime Timestamp { get; } = DateTime.UtcNow;
}
