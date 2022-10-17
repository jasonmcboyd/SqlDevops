using Microsoft.SqlServer.Dac;
using Microsoft.SqlServer.Dac.Model;
using SqlDevOps.PSCmdlets.BasePSCmdlets;
using SqlDevOps.Utilities;
using System.IO;
using System.Management.Automation;
using System.Threading;

namespace SqlDevOps.PSCmdlets
{
  [Cmdlet(VerbsData.Publish, PSCmdletNouns.TSqlModel)]
  [OutputType(typeof(PublishResult))]
  public class PublishTSqlModelPSCmdlet : BaseDbConnectionPSCmdlet
  {

    #region Parameters

    [Parameter(
      Mandatory = true,
      Position = 0,
      ValueFromPipeline = true)]
    public TSqlModel? Model { get; set; }

    [Parameter(
      Mandatory = false,
      Position = 1)]
    public DacDeployOptions? DacDeployOptions { get; set; }

    // TODO: Does this work?
    [Parameter()]
    public SwitchParameter IgnoreValidationErrors { get; set; }

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
      var packageMetadata = new PackageMetadata();

      using var stream = new MemoryStream();

      var packageOptions = new PackageOptions();

      // TODO: Figure out _exactly_ how IgnoreValidationErrors works.
      if (IgnoreValidationErrors)
        packageOptions.IgnoreValidationErrors = new string[] { "*" };

      DacPackageExtensions.BuildPackage(stream, Model, packageMetadata, packageOptions);

      var package = DacPackage.Load(stream);

      var publishOptions = new PublishOptions()
      {
        DeployOptions = DacDeployOptions,
        CancelToken = CancellationTokenSource.Token,
      };

      var result = service.Publish(package, connectionStringBuilder.InitialCatalog, publishOptions);

      WriteObject(result);
    }
  }
}
