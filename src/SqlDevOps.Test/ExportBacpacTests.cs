using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlDevOps.PSCmdlets;
using SqlDevOps.Test.Extensions;
using Microsoft.PowerShell.Commands;
using SqlDevOps.Test.Utilities;
using SqlDevOps.Test.Sql;
using static SqlDevOps.Test.Initialize;

namespace SqlDevOps.Test
{
  [TestClass]
  public class ExportBacpacTests
  {
    [TestMethod]
    [Ignore]
    public async Task Invoke()
    {
      // Arrange
      var db = new TestDatabase().WithTable().WithValidStoredProcedure();
      await Container.CreateDatabaseAsync(db);

      using var powerShell = PowerShellFactory.CreateInstance();
      
      // Act
      powerShell
        .BuildCommand<ExportBacpacPSCmdlet>()
        .AddParameter(x => x.ConnectionString, Container.GetSecureConnectionString(db.Name))
        .AddParameter(x => x.Path, @"C:\wrk\test.bacpac");

      var file = powerShell.InvokeScalarCommand<FileHashInfo>();

      // Assert
      Assert.IsNotNull(file);
    }
  }
}
