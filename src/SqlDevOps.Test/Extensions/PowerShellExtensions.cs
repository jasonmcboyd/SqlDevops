using System.Collections.Generic;
using System.Linq;
using SqlDevOps.Test.Utilities;

using System.Management.Automation;

namespace SqlDevOps.Test.Extensions
{
  static class PowerShellExtensions
  {
    public static PowerShellCommandBuilder<TCmdlet> BuildCommand<TCmdlet>(this PowerShell powerShell)
      where TCmdlet : Cmdlet
    {
      var builder = new PowerShellCommandBuilder<TCmdlet>(powerShell);

      return builder;
    }

    public static TResult InvokeScalarCommand<TResult>(this PowerShell powerShell)
    {
      var result = powerShell.Invoke();

      return (TResult)result.Single().BaseObject;
    }

    public static IEnumerable<TResult> InvokeCommand<TResult>(this PowerShell powerShell)
    {
      return powerShell.Invoke().Select(x => x.BaseObject).OfType<TResult>();
    }
  }
}
