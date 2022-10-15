using System.Management.Automation;
using System.Security;
using Microsoft.SqlServer.Dac;

namespace SqlDevOps.PSCmdlets
{
  [Cmdlet(VerbsCommon.New, PSCmdletNouns.DacDeployOptions)]
  [OutputType(typeof(SecureString))]
  public class NewDacDeployOptions : PSCmdlet
  {
    protected override void ProcessRecord()
    {
      WriteObject(new DacDeployOptions());
    }
  }
}
