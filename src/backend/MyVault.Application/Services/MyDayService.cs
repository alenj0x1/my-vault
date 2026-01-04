using System;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using MyVault.Application.Interfaces.Services;
using MyVault.Application.Models.Requests;
using MyVault.Application.Models.Responses;
using MyVault.Domain.Entities;
using MyVault.Domain.Enums;
using MyVault.Shared.Constants;

namespace MyVault.Application.Services;

public class MyDayService(IMemoryCache memoryCache, IConfiguration configuration) : IMyDayService
{
    public GenericResponse<Day> Create(CreateDayRequest model)
    {
        throw new NotImplementedException();
    }

    public GenericResponse<List<Day>> Get(int id)
    {
        throw new NotImplementedException();
    }

    public GenericResponse<Day?> Get(BaseRequest model)
    {
        throw new NotImplementedException();
    }

    public async Task InitCache(List<Day> data)
    {
        await memoryCache.GetOrCreateAsync(CacheKey.DAY_DATA, async (cacheEntry) =>
        {
            cacheEntry.SlidingExpiration = TimeSpan.FromHours(1);
            return data;
        });
    }

    public async Task<List<Day>> InitData()
    {
        try
        {
            List<Day> data = [];

            var currentLine = 0;

            var currentDay = 0;
            Day? day = null;

            var lines = await File.ReadAllLinesAsync(configuration[ConfigurationProperty.DAY_FILE_PATH]
                ?? throw new Exception(ExceptionMessage.CONFIGURATION_PROPERTY_NOT_FOUND(ConfigurationProperty.DAY_FILE_PATH)));
            foreach (var line in lines)
            {
                currentLine++;

                var seperatorDate = configuration[ConfigurationProperty.DAY_FILE_SEPARATOR_DATE] ?? "-";
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
                        Items = [],
                        Errors = dateParsed is null
                            ? new Dictionary<string, string>() { { "Date", ExceptionMessage.INCORRECT_FORMAT } }
                            : []
                    };
                }

                Dictionary<string, string> properties = [];
                foreach (var item in line.Split(configuration[ConfigurationProperty.DAY_FILE_SEPARATOR_ITEM] ?? ";"))
                {
                    var property = item.Split(configuration[ConfigurationProperty.DAY_FILE_SEPARATOR_PROPERTY] ?? "=");
                    if (property.Length != 2) continue;

                    properties.Add(property[0], property[1]);
                }

                if (day is not null && properties.Count > 0)
                {
                    day.Items.Add(new DayItem
                    {
                        Id = currentLine,
                        Identifier = DayIdentifier.Coding,
                        Time = properties["time"],
                        Type = DayType.English,
                        Note = properties["note"]
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
