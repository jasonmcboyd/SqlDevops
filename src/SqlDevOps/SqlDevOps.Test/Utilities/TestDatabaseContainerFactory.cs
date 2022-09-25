using System.Threading.Tasks;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.Data.SqlClient;
using SqlDevOps.Test.Extensions;

namespace SqlDevOps.Test.Utilities
{
  static class TestDatabaseContainerFactory
  {
    public static async Task<MsSqlTestcontainer> CreateContainerAsync()
    {
      var builder =
        new TestcontainersBuilder<MsSqlTestcontainer>()
        .WithDatabase(new MsSqlTestcontainerConfiguration
        {
          Password = "Password1"
        });

      var container = builder.Build();

      await container.StartAsync();

      await CreateDataBaseAsync(container);
      await AddTableAsync(container);
      await AddInvalidStoredProcedureAsync(container);

      return container;
    }

    private static async Task CreateDataBaseAsync(MsSqlTestcontainer container)
    {
      await using var connection = new SqlConnection(container.GetConnectionString());
      await using var command = connection.CreateCommand();

      connection.Open();
      command.CommandText = "create database Test";

      await command.ExecuteNonQueryAsync();

      container.Database = "Test";
    }

    private static async Task AddTableAsync(MsSqlTestcontainer container)
    {
      await using var connection = new SqlConnection(container.GetConnectionString());
      await using var command = connection.CreateCommand();
      
      connection.Open();

      command.CommandText = @"
        create table Address
        (
          [Id] int identity (1, 1) not null,
          [StreetAddress] varchar (50) not null,
        )";
      await command.ExecuteNonQueryAsync();
      
      command.CommandText = @"
        insert into [Test].[dbo].[Address] ([StreetAddress])
        values ('2039 Sweet Birch Lane')";
      await command.ExecuteNonQueryAsync();
    }

    private static async Task AddInvalidStoredProcedureAsync(MsSqlTestcontainer container)
    {
      await using var connection = new SqlConnection(container.GetConnectionString());
      await using var command = connection.CreateCommand();

      connection.Open();

      command.CommandText = @"
        create procedure InvalidProcedure
        as
        begin
          select
            *
          from
            [SomeDatabase].[dbo].[Address]
        end";
      await command.ExecuteNonQueryAsync();
    }
  }
}
