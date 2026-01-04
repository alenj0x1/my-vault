using System;
using MyVault.Application.Models.Requests;
using MyVault.Application.Models.Responses;
using MyVault.Domain.Entities;

namespace MyVault.Application.Interfaces.Services;

public interface IMyDayService
{
    GenericResponse<Day> Create(CreateDayRequest model);
    GenericResponse<List<Day>> Get(int id);
    GenericResponse<Day?> Get(BaseRequest model);
    Task InitCache(List<Day> data);
    Task<List<Day>> InitData();
}
