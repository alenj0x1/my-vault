using System;
using Dapper;
using Microsoft.Data.Sqlite;

namespace MyVault.Infrastructure.Persistence.Sqlite;

public class Initializer(string connectionString)
{
    public async Task ExecuteAsync()
    {
        try
        {
            using var connection = new SqliteConnection(connectionString);
            await connection.OpenAsync();


            using var cmd = connection.CreateCommand();
            cmd.CommandText = """
                PRAGMA journal_mode = WAL;
                PRAGMA synchronous = NORMAL;
            """;
            await cmd.ExecuteNonQueryAsync();

            using var tx = await connection.BeginTransactionAsync();
            await connection.ExecuteAsync("""
                create table if not exists days (
                    id integer not null primary key,
                    date date not null
                )
            """, transaction: tx);

            await connection.ExecuteAsync("""
                create table if not exists days_items (
                    id integer not null primary key,
                    day_id integer not null references days,
                    identifier integer not null,
                    time varchar(10) not null,
                    type integer not null,
                    sub_type integer,
                    note text not null
                )
            """, transaction: tx);

            await tx.CommitAsync();


        }
        catch (System.Exception)
        {

            throw;
        }
    }
}
