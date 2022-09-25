using DotNet.Testcontainers.Containers;
using Microsoft.Data.SqlClient;

namespace SqlDevOps.Test.Extensions
{
  public static class MsSqlTestcontainerExtensions
  {
    public static string GetConnectionString(this MsSqlTestcontainer container)
    {
      var builder = new SqlConnectionStringBuilder
      {
        ConnectionString = container.ConnectionString,
        Encrypt = false
      };
      return builder.ConnectionString;
    }
  }
}
