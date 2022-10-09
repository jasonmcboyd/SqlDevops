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

      return container;
    }
  }
}
