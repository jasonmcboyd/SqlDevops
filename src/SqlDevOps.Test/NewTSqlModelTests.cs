using System.Threading.Tasks;
using Microsoft.SqlServer.Dac.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlDevOps.PSCmdlets;
using SqlDevOps.Test.Extensions;
using SqlDevOps.Test.Utilities;
using static SqlDevOps.Test.Initialize;
using SqlDevOps.Test.Sql;

namespace SqlDevOps.Test
{
  [TestClass]
  public class NewTSqlModelTests
  {
    [TestMethod]
    public async Task TestMethod1()
    {
      // Arrange
      var db = new TestDatabase().WithTable().WithValidStoredProcedure();
      await Container.CreateDatabaseAsync(db);

      using var powerShell = PowerShellFactory.CreateInstance();

      powerShell
        .BuildCommand<NewTSqlModelPSCmdlet>()
        .AddParameter(x => x.ConnectionString, Container.GetSecureConnectionString(db.Name));

      // Act
      var model = powerShell.InvokeScalarCommand<TSqlModel>();

      // Assert
      Assert.IsNotNull(model);
    }
  }
}
