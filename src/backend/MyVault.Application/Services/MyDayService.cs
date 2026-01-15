using System;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using MyVault.Application.Interfaces.Services;
using MyVault.Application.Models.Requests;
using MyVault.Application.Models.Requests.Day;
using MyVault.Application.Models.Responses;
using MyVault.Domain.Entities;
using MyVault.Domain.Enums;
using MyVault.Domain.Interfaces.Repositories;
using MyVault.Shared;
using MyVault.Shared.Constants;

namespace MyVault.Application.Services;

public class MyDayService(
    IMemoryCache memoryCache,
    IConfiguration configuration,
    IDayRepository dayRepository
) : IMyDayService
{
    private readonly IDayRepository _dayRepository = dayRepository;

    public async Task<GenericResponse<Day?>> CreateAsync(CreateDayRequest model)
    {
        try
        {
            var day = await _dayRepository.CreateAsync(new Day
            {
                Date = model.Date
            });

            return new GenericResponse<Day?>
            {
                Data = day
            };
        }
        catch
        {
            return new GenericResponse<Day?>
            {
                Data = null
            };
        }
    }

    public async Task<GenericResponse<Day?>> GetAsync(int id)
    {
        try
        {
            var day = await _dayRepository.GetAsync(id);

            return new GenericResponse<Day?>
            {
                Data = day
            };
        }
        catch
        {
            return new GenericResponse<Day?>
            {
                Data = null
            };
        }
    }

    public async Task<GenericResponse<List<Day>>> GetAsync(BaseRequest model)
    {
        try
        {
            var days = await _dayRepository.GetAsync(limit: model.Limit, offset: model.Offset);

            return new GenericResponse<List<Day>>
            {
                Data = days
            };
        }
        catch
        {
            return new GenericResponse<List<Day>>
            {
                Data = null
            };
        }
    }

    public async Task<GenericResponse<DayItem>> CreateItemAsync(CreateDayItemRequest model)
    {
        try
        {
            var repositoryData = await _dayRepository.CreateItemAsync(new DayItem
            {
                DayId = model.DayId,
                Identifier = model.Identifier,
                Time = model.Time ?? "",
                Type = model.Type,
                SubType = model.SubType,
                Note = model.Note,
            });

            return new GenericResponse<DayItem>
            {
                Data = repositoryData
            };
        }
        catch
        {
            return new GenericResponse<DayItem>
            {
                Data = null
            };
        }
    }

    public Task<GenericResponse<DayItem?>> UpdateItemAsync(int id, UpdateDayItemRequest model)
    {
        throw new NotImplementedException();
    }

    public async Task<GenericResponse<bool>> RemoveItemAsync(int id)
    {
        try
        {
            var repositoryData = await _dayRepository.DeleteItemAsync(id);
            return new GenericResponse<bool>
            {
                Data = repositoryData
            };
        }
        catch
        {
            return new GenericResponse<bool>
            {
                Data = false
            };
        }
    }

    public async Task InitCacheAsync(List<Day> data)
    {
        await memoryCache.GetOrCreateAsync(CacheKey.DAY_DATA, async (cacheEntry) =>
        {
            cacheEntry.SlidingExpiration = TimeSpan.FromHours(1);
            return data;
        });
    }

    public async Task<List<Day>> InitDataAsyncDeprecated()
    {
        try
        {
            List<Day> data = [];

            var currentLine = 0;

            var currentDay = 0;
            Day? day = null;

            var lines = await File.ReadAllLinesAsync(configuration[ConfigurationProperty.DAY_FILE_PATH_DEPRECATED]
                ?? throw new Exception(ExceptionMessage.CONFIGURATION_PROPERTY_NOT_FOUND(ConfigurationProperty.DAY_FILE_PATH_DEPRECATED)));
            foreach (var line in lines)
            {
                currentLine++;

                var seperatorDate = configuration[ConfigurationProperty.DAY_FILE_SEPARATOR_DATE_DEPRECATED] ?? "-";
                if (line.StartsWith(seperatorDate))
                {
                    currentDay++;

                    var date = line.Replace(seperatorDate, "");
                    DateTime? dateParsed = null;

                    if (day is not null)
                    {
                        data.Add(day);
                    }

                    try
                    {
                        dateParsed = Convert.ToDateTime(date);
                    }
                    catch { }

                    day = new Day()
                    {
                        Id = currentDay,
                        Date = dateParsed,
                        Items = []
                    };
                }

                Dictionary<string, string> properties = [];
                foreach (var item in line.Split(configuration[ConfigurationProperty.DAY_FILE_SEPARATOR_ITEM_DEPRECATED] ?? ";"))
                {
                    var property = item.Split(configuration[ConfigurationProperty.DAY_FILE_SEPARATOR_PROPERTY_DEPRECATED] ?? "=");
                    if (property.Length != 2) continue;

                    properties.Add(property[0], property[1]);
                }

                if (day is not null && properties.Count > 0)
                {

                    Parser.EnumToString(properties.GetValueOrDefault("identifier") ?? "", out DayIdentifier identifier);
                    Parser.EnumToString(properties.GetValueOrDefault("type") ?? "", out DayType type);
                    Parser.EnumToString(properties.GetValueOrDefault("subType") ?? "", out DayType subType);

                    day.Items.Add(new DayItem
                    {
                        Id = currentLine,
                        DayId = currentDay,
                        Identifier = (int)identifier,
                        Time = properties.GetValueOrDefault("time") ?? "",
                        Type = (int)type,
                        SubType = subType.CompareTo(DayType.UnespecifiedOrUnknown) == 0 ? null : (int)subType,
                        Note = properties.GetValueOrDefault("note") ?? ""
                    });
                }

                if (currentLine == lines.Length && day is not null)
                {
                    data.Add(day);
                }
            }

            return data;
        }
        catch
        {

            throw;
        }
    }
}
