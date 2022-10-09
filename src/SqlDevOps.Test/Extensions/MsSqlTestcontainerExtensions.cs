using DotNet.Testcontainers.Containers;
using Microsoft.Data.SqlClient;

using SqlDevOps.Extensions;
using SqlDevOps.Test.Sql;

using System.Security;
using System.Threading.Tasks;

namespace SqlDevOps.Test.Extensions
{
  internal static class MsSqlTestcontainerExtensions
  {
    internal static string GetConnectionString(this MsSqlTestcontainer container, string databaseName = null)
    {
      var builder = new SqlConnectionStringBuilder
      {
        ConnectionString = container.ConnectionString,
        Encrypt = false,
      };

      if (databaseName?.Length > 0)
        builder.InitialCatalog = databaseName;

      return builder.ConnectionString;
    }

    internal static SecureString GetSecureConnectionString(this MsSqlTestcontainer container, string databaseName = null) => container.GetConnectionString(databaseName).ToSecureString();

    internal static Task CreateDatabaseAsync(this MsSqlTestcontainer container, TestDatabase testDatabase) => testDatabase.CreateAsync(container.GetConnectionString());
  }
}
