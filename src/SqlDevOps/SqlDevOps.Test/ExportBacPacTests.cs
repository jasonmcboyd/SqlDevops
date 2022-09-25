using System;
using System.Diagnostics;
using System.Threading.Tasks;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlDevOps.PSCmdlets;
using SqlDevOps.Test.Extensions;
using Microsoft.PowerShell.Commands;
using SqlDevOps.Test.Utilities;
using SqlDevOps.Extensions;

namespace SqlDevOps.Test
{
  [TestClass]
  public class ExportBacpacTests
  {
    [TestMethod]
    public async Task Invoke()
    {
      // Arrange
      await using var container = await TestDatabaseContainerFactory.CreateContainerAsync();

      using var powerShell = PowerShellFactory.CreateInstance();
      
      // Act
      powerShell
        .BuildCommand<ExportBacpacPSCmdlet>()
        .AddParameter(x => x.ConnectionString, container.GetConnectionString().ToSecureString())
        .AddParameter(x => x.Path, @"C:\wrk\test.bacpac");

      var file = powerShell.InvokeScalarCommand<FileHashInfo>();

      // Assert
      Assert.IsNotNull(file);
    }
  }
}
