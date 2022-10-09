using SqlDevOps.PSCmdlets;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace SqlDevOps.Test.Utilities
{
  static class PowerShellFactory
  {
    public static PowerShell CreateInstance()
    {
      var state = InitialSessionState.CreateDefault();

      var modulePath = typeof(NewSecureConnectionStringPSCmdlet).Assembly.Location;

      state.ImportPSModule(modulePath);

      return PowerShell.Create(state);
    }
  }
}
