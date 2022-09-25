using System;
using System.Threading.Tasks;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Dac.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlDevOps.PSCmdlets;
using SqlDevOps.Test.Extensions;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Security;
using Microsoft.PowerShell.Commands;
using SqlDevOps.Extensions;
using SqlDevOps.Test.Utilities;

namespace SqlDevOps.Test
{
  [TestClass]
  public class GetTSqlObjectTests
  {
    [TestMethod]
    public async Task Invoke_Table_TablesReturned()
    {
      // Arrange
      await using var container = await TestDatabaseContainerFactory.CreateContainerAsync();
      
      using var powerShell = PowerShellFactory.CreateInstance();

      var cs = container.GetConnectionString();

      powerShell
        .BuildCommand<NewTSqlModelPSCmdlet>()
        .AddParameter(x => x.ConnectionString, container.GetConnectionString().ToSecureString());
      
      // Act
      var model = powerShell.InvokeScalarCommand<TSqlModel>();

      powerShell.Commands.Clear();

      powerShell
        .BuildCommand<GetTSqlObjectPSCmdlet>()
        .AddParameter(x => x.Model, model)
        .AddParameter(x => x.ObjectType, new [] { "Table" });

      var tables = powerShell.InvokeCommand<TSqlObject>().ToArray();

      // Assert
      CollectionAssert.AreEquivalent(new [] { "[dbo].[Address]" }, tables.Select(x => x.Name.ToString()).ToArray());
    }

    [TestMethod]
    public async Task Invoke_StoredProcedure_StoredProcedureReturned()
    {
      // Arrange
      await using var container = await TestDatabaseContainerFactory.CreateContainerAsync();

      using var powerShell = PowerShellFactory.CreateInstance();

      powerShell
        .BuildCommand<NewTSqlModelPSCmdlet>()
        .AddParameter(x => x.ConnectionString, container.GetConnectionString().ToSecureString());

      // Act
      var model = powerShell.InvokeScalarCommand<TSqlModel>();

      powerShell.Commands.Clear();

      powerShell
        .BuildCommand<GetTSqlObjectPSCmdlet>()
        .AddParameter(x => x.Model, model)
        .AddParameter(x => x.ObjectType, new[] { "StoredProcedure" });

      var storedProcedures = powerShell.InvokeCommand<TSqlObject>().ToArray();

      // Assert
      CollectionAssert.AreEquivalent(new[] { "[dbo].[InvalidProcedure]" }, storedProcedures.Select(x => x.Name.ToString()).ToArray());
    }
  }
}
