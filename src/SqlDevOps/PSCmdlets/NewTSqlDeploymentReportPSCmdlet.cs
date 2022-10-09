using Microsoft.SqlServer.Dac;
using Microsoft.SqlServer.Dac.Model;
using SqlDevOps.PSCmdlets.BasePSCmdlets;
using SqlDevOps.Utilities;
using System.IO;
using System.Management.Automation;

namespace SqlDevOps.PSCmdlets
{
  [Cmdlet(VerbsCommon.New, PSCmdletNouns.TSqlDeploymentReport)]
  // TODO:
  [OutputType(typeof(string))]
  public class NewTSqlDeploymentReportPSCmdlet : BaseDbConnectionPSCmdlet
  {

    #region Parameters

    [Parameter(
      Mandatory = true,
      Position = 0,
      ValueFromPipeline = true)]
    public TSqlModel? Model { get; set; }

    #endregion Parameters

    protected override void ProcessRecord()
    {
      var connectionStringBuilder =
        SqlConnectionStringBuilderBuilder.Build(
          ConnectionString,
          Server,
          Database,
          Credential,
          IntegratedSecurity);

      var service = new DacServices(connectionStringBuilder.ToString());

      using (var dacpacStream = new MemoryStream())
      {
        // TODO:
        var packageMetadata = new PackageMetadata();
        DacPackageExtensions.BuildPackage(dacpacStream, Model, packageMetadata);
        CancellationTokenSource.Token.ThrowIfCancellationRequested();
        var dacpac = DacPackage.Load(dacpacStream);
        CancellationTokenSource.Token.ThrowIfCancellationRequested();
        //var options = new DacDeployOptions();
        WriteVerbose("Generating deployment report.");
        var script = service.GenerateDeployReport(dacpac, connectionStringBuilder.InitialCatalog, null, CancellationTokenSource.Token);
        WriteObject(script);
      }
    }

  }
}
