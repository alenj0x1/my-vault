using System;
using System.ComponentModel.DataAnnotations;
using MyVault.Domain.Enums;

namespace MyVault.Application.Models.Requests;

public class CreateDayRequest
{

    [Required]
    public DateTime Date { get; set; }
}
