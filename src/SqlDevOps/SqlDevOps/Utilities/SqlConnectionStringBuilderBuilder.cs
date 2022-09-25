using Microsoft.Data.SqlClient;
using System.Management.Automation;
using System.Security;
using SqlDevOps.Extensions;

namespace SqlDevOps.Utilities
{
  internal static class SqlConnectionStringBuilderBuilder
  {
    internal static SqlConnectionStringBuilder Build(
      SecureString? connectionString,
      string? server,
      string? database,
      PSCredential? credentials,
      bool integratedSecurity)
    {
      var result = new SqlConnectionStringBuilder();
      
      if (connectionString is not null)
        result.ConnectionString = connectionString.ToPlainTextString();
      
      if (!string.IsNullOrWhiteSpace(server))
        result.DataSource = server;
      
      if (!string.IsNullOrWhiteSpace(database))
        result.InitialCatalog = database;
      
      if (integratedSecurity)
        result.IntegratedSecurity = true;
      
      if (credentials is not null)
      { 
        result.UserID = credentials.UserName;
        result.Password = credentials.GetNetworkCredential().Password;
      }

      return result;
    }
  }
}
