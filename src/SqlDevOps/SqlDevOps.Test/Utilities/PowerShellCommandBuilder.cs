using System;
using System.Linq.Expressions;
using System.Management.Automation;
using System.Reflection;

namespace SqlDevOps.Test.Utilities
{
  class PowerShellCommandBuilder<TCmdlet>
    where TCmdlet : Cmdlet
  {
    public PowerShellCommandBuilder(PowerShell powerShell)
    {
      PowerShell = powerShell;
      powerShell.AddCommand(CmdletUtilities.GetName<TCmdlet>());
    }

    private PowerShell PowerShell { get; }

    public PowerShellCommandBuilder<TCmdlet> AddParameter<TParameter>(
      Expression<Func<TCmdlet, TParameter>> parameterSelector,
      TParameter value)
    {
      var type = typeof(TCmdlet);

      var member = parameterSelector.Body as MemberExpression;
      
      var propInfo = member.Member as PropertyInfo;

      var parameterName = propInfo.Name;

      PowerShell.AddParameter(parameterName, value);

      return this;
    }

    public PowerShellCommandBuilder<TCmdlet> AddParameter(
      Expression<Func<TCmdlet, SwitchParameter>> parameterSelector,
      bool value)
    {
      var type = typeof(TCmdlet);

      var member = parameterSelector.Body as MemberExpression;

      var propInfo = member.Member as PropertyInfo;

      var parameterName = propInfo.Name;

      PowerShell.AddParameter(parameterName, value);

      return this;
    }
  }
}
