using System;
using MyVault.Application.Models.Requests;
using MyVault.Application.Models.Requests.Day;
using MyVault.Application.Models.Responses;
using MyVault.Domain.Entities;

namespace MyVault.Application.Interfaces.Services;

public interface IMyDayService
{
    Task<GenericResponse<Day?>> CreateAsync(CreateDayRequest model);
    Task<GenericResponse<Day?>> GetAsync(int id);
    Task<GenericResponse<List<Day>>> GetAsync(BaseRequest model);
    Task<GenericResponse<DayItem>> CreateItemAsync(CreateDayItemRequest model);
    Task<GenericResponse<DayItem?>> UpdateItemAsync(int id, UpdateDayItemRequest model);
    Task<GenericResponse<bool>> RemoveItemAsync(int id);
    Task InitCacheAsync(List<Day> data);
    Task<List<Day>> InitDataAsyncDeprecated();
}
