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

    public async Task<Day> CreateAsync(Day day)
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

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var query = await connection.ExecuteAsync("""
                delete from days where id = @Id
            """, new
            {
                Id = id
            });

            return query > 0;
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    public async Task<Day?> GetAsync(int id)
    {
        try
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var day = await connection.QuerySingleAsync<Day?>("""
                select id, date from days where id = @Id
            """, new
            {
                Id = id
            });

            if (day is not null)
            {

                var items = await connection.QueryAsync<DayItem>("""
                    select id, day_id, identifier, time, type, sub_type, note from days_items where day_id = @DayId
                """, new
                {
                    DayId = day.Id
                });

                day.Items = [.. items];
            }

            return day;
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    public async Task<List<Day>> GetAsync(int limit = 100, int offset = 0)
    {
        try
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var days = (await connection.QueryAsync<Day>("""
                select id, date from days limit @Limit offset @Offset
            """, new
            {
                Limit = limit,
                Offset = offset
            })).ToList();

            foreach (var day in days)
            {
                var items = await connection.QueryAsync<DayItem>("""
                    select id, day_id, identifier, time, type, sub_type, note from days_items where day_id = @DayId
                """, new
                {
                    DayId = day.Id
                });

                day.Items = [.. items];
            }

            return days;
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    public async Task<Day?> UpdateAsync(Day day)
    {
        try
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var query = await connection.ExecuteAsync("""
                update days set date = @Date where id = @Id
            """, new
            {
                Id = day.Id,
                Date = day.Date
            });

            if (query > 0)
            {
                return await GetAsync(day.Id);
            }

            return day;
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    public async Task<DayItem> CreateItemAsync(DayItem item)
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

    public async Task<DayItem?> UpdateItemAsync(DayItem item)
    {
        try
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var query = await connection.ExecuteAsync("""
                update days_items 
                set 
                    day_id = @DayId,
                    identifier = @Identifier,
                    type = @Type,
                    sub_type = @SubType,
                    note = @Note,
                    time = @Time
                where id = @Id
            """, new
            {
                Id = item.Id,
                DayId = item.DayId,
                Identifier = item.Identifier,
                Type = item.Type,
                SubType = item.SubType,
                Note = item.Note,
                Time = item.Time
            });

            return item;
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    public async Task<bool> DeleteItemAsync(int id)
    {
        try
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var query = await connection.ExecuteAsync("""
                delete from days_items where id = @Id
            """, new
            {
                Id = id
            });

            return query > 0;
        }
        catch (System.Exception)
        {
            throw;
        }
    }
}
