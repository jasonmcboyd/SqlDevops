using System.Threading.Tasks;
using Microsoft.SqlServer.Dac.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlDevOps.PSCmdlets;
using SqlDevOps.Test.Extensions;
using SqlDevOps.Extensions;
using SqlDevOps.Test.Utilities;

namespace SqlDevOps.Test
{
  [TestClass]
  public class NewTSqlModelTests
  {
    [TestMethod]
    public async Task TestMethod1()
    {
      // Arrange
      await using var container = await TestDatabaseContainerFactory.CreateContainerAsync();

      using var powerShell = PowerShellFactory.CreateInstance();

      powerShell
        .BuildCommand<NewTSqlModelPSCmdlet>()
        .AddParameter(x => x.ConnectionString, container.GetConnectionString().ToSecureString());

      // Act
      var model = powerShell.InvokeScalarCommand<TSqlModel>();

      // Assert
      Assert.IsNotNull(model);
    }
  }
}
