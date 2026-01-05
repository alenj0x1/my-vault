using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using MyVault.Domain.Entities;
using MyVault.Domain.Interfaces.Repositories;
using MyVault.Shared.Constants;

namespace MyVault.Infrastructure.Persistence.Sqlite.Repositories;

public class DayRepository : IDayRepository
{
    public string _connectionString;

    public DayRepository(IConfiguration configuration)
    {
        _connectionString = configuration[ConfigurationProperty.CONNECTION_STRING_DATABASE]
            ?? throw new Exception(ExceptionMessage.CONFIGURATION_PROPERTY_NOT_FOUND(ConfigurationProperty.CONNECTION_STRING_DATABASE));
    }

    public async Task<Day> Create(Day day)
    {
        try
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            using var tx = await connection.BeginTransactionAsync();

            var query = await connection.QuerySingleAsync<Day>("""
                insert into days (date)
                values (@Date)
                returning id, date
            """, new { Date = day.Date }, transaction: tx);

            await tx.CommitAsync();

            day.Id = query.Id;

            return day;
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    public async Task<DayItem> CreateItem(DayItem item)
    {
        try
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            using var tx = await connection.BeginTransactionAsync();

            var query = await connection.QuerySingleAsync<DayItem>("""
                insert into days_items (day_id, identifier, time, type, note, sub_type)
                values (@DayId, @Identifier, @Time, @Type, @Note, @SubType)
                returning id
            """, new
            {
                DayId = item.DayId,
                Identifier = item.Identifier,
                Time = item.Time,
                Type = item.Type,
                Note = item.Note,
                SubType = item.SubType,
            }, transaction: tx
                );

            await tx.CommitAsync();

            item.Id = query.Id;

            return item;
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    public bool Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Day? Get(int id)
    {
        throw new NotImplementedException();
    }

    public List<Day> Get(int limit = 100, int skip = 0)
    {
        throw new NotImplementedException();
    }

    public Day? Update(Day day)
    {
        throw new NotImplementedException();
    }
}
