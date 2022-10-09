using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlDevOps.PSCmdlets;
using SqlDevOps.Test.Extensions;
using System.Management.Automation;
using System.Security;
using SqlDevOps.Extensions;
using SqlDevOps.Test.Utilities;

namespace SqlDevOps.Test
{
  [TestClass]
  public class NewSecureConnectionStringTests
  {
    [TestMethod]
    public void Invoke_IntegreatedSecurity_CorrectValueReturned()
    {
      // Arrange
      using var powerShell = PowerShellFactory.CreateInstance();

      powerShell
        .BuildCommand<NewSecureConnectionStringPSCmdlet>()
        .AddParameter(x => x.Server, "localhost")
        .AddParameter(x => x.Database, "Test")
        .AddParameter(x => x.IntegratedSecurity, true);

      // Act
      var connectionString = powerShell.InvokeScalarCommand<SecureString>();

      // Assert
      Assert.AreEqual("Data Source=localhost;Initial Catalog=Test;Integrated Security=True", connectionString.ToPlainTextString());
    }

    [TestMethod]
    public void Invoke_WithCredentials_CorrectValueReturned()
    {
      // Arrange
      using var powerShell = PowerShellFactory.CreateInstance();

      powerShell
        .BuildCommand<NewSecureConnectionStringPSCmdlet>()
        .AddParameter(x => x.Server, "localhost")
        .AddParameter(x => x.Database, "Test")
        .AddParameter(x => x.Credential, new PSCredential("user", "password".ToSecureString()));

      // Act
      var connectionString = powerShell.InvokeScalarCommand<SecureString>();

      // Assert
      Assert.AreEqual("Data Source=localhost;Initial Catalog=Test;User ID=user;Password=password", connectionString.ToPlainTextString());
    }
  }
}
