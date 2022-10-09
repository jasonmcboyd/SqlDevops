using System.Threading.Tasks;
using Microsoft.SqlServer.Dac.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlDevOps.PSCmdlets;
using SqlDevOps.Test.Extensions;
using System.Linq;
using SqlDevOps.Extensions;
using SqlDevOps.Test.Utilities;
using static SqlDevOps.Test.Initialize;
using SqlDevOps.Test.Sql;

namespace SqlDevOps.Test
{
  [TestClass]
  public class GetTSqlObjectTests
  {
    [TestMethod]
    public async Task Invoke_Table_TablesReturned()
    {
      // Arrange
      var db = new TestDatabase().WithTable();
      await Container.CreateDatabaseAsync(db);
      
      using var powerShell = PowerShellFactory.CreateInstance();

      powerShell
        .BuildCommand<NewTSqlModelPSCmdlet>()
        .AddParameter(x => x.ConnectionString, Container.GetSecureConnectionString(db.Name))
        .AddParameter(x => x.Database, db.Name);
      
      // Act
      var model = powerShell.InvokeScalarCommand<TSqlModel>();

      powerShell.Commands.Clear();

      powerShell
        .BuildCommand<GetTSqlObjectPSCmdlet>()
        .AddParameter(x => x.Model, model)
        .AddParameter(x => x.ObjectType, new [] { "Table" });

      var tables = powerShell.InvokeCommand<TSqlObject>().ToArray();

      var expected = db.Tables.Select(x => $"[dbo].[{x.Name}]").ToArray();
      var actual = tables.Select(x => x.Name.ToString()).ToArray();

      // Assert
      CollectionAssert.AreEquivalent(expected, actual);
    }

    [TestMethod]
    public async Task Invoke_StoredProcedure_StoredProcedureReturned()
    {
      // Arrange
      var db = new TestDatabase().WithValidStoredProcedure();
      await Container.CreateDatabaseAsync(db);

      using var powerShell = PowerShellFactory.CreateInstance();

      powerShell
        .BuildCommand<NewTSqlModelPSCmdlet>()
        .AddParameter(x => x.ConnectionString, Container.GetSecureConnectionString(db.Name))
        .AddParameter(x => x.Database, db.Name);

      // Act
      var model = powerShell.InvokeScalarCommand<TSqlModel>();

      powerShell.Commands.Clear();

      powerShell
        .BuildCommand<GetTSqlObjectPSCmdlet>()
        .AddParameter(x => x.Model, model)
        .AddParameter(x => x.ObjectType, new[] { "StoredProcedure" });

      var storedProcedures = powerShell.InvokeCommand<TSqlObject>().ToArray();

      // Assert
      CollectionAssert.AreEquivalent(db.StoredProcedures.Select(x => $"[dbo].[{x.Name}]").ToArray(), storedProcedures.Select(x => x.Name.ToString()).ToArray());
    }
  }
}
