using System;
using System.Diagnostics;
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
using SqlDevOps.Test.Utilities;
using SqlDevOps.Extensions;
using static SqlDevOps.Test.Utilities.TestHelper;
using SqlDevOps.Test.Sql;
using static SqlDevOps.Test.Initialize;

namespace SqlDevOps.Test
{
  [TestClass]
  public class RemoveTSqlObjectTests
  {
    [TestMethod]
    public async Task Invoke()
    {
      // Arrange
      using var powerShell = PowerShellFactory.CreateInstance();

      var db = new TestDatabase().WithValidStoredProcedure();
      await Container.CreateDatabaseAsync(db);
      
      powerShell
        .BuildCommand<NewTSqlModelPSCmdlet>()
        .AddParameter(x => x.ConnectionString, Container.GetSecureConnectionString(db.Name));

      var model = powerShell.InvokeScalarCommand<TSqlModel>();

      var storedProcedures = model.GetStoredProcedures().ToArray();

      // Preconditions
      Assume(
        () => Assert.IsTrue(storedProcedures.Length > 0),
        "Expecting one ore more stored procedures to exist.");

      // Act
      powerShell.Commands.Clear();

      powerShell
        .BuildCommand<RemoveTSqlObjectPSCmdlet>()
        .AddParameter(x => x.Model, model)
        .AddParameter(x => x.TSqlObject, storedProcedures);

      powerShell.Invoke();

      // Assert
      Assert.AreEqual(0, model.GetStoredProcedures().Count());
    }
  }
}
