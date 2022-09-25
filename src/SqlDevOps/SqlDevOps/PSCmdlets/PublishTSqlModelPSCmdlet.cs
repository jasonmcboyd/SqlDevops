using Microsoft.SqlServer.Dac;
using Microsoft.SqlServer.Dac.Model;
using SqlDevOps.PSCmdlets.BasePSCmdlets;
using SqlDevOps.Utilities;
using System.IO;
using System.Management.Automation;

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

    public SwitchParameter IgnoreValidationErrors { get; set; }

    #endregion Parameters

    // This method will be called for each input received from the pipeline to this cmdlet; if no input is received, this method is not called
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

      DacPackageExtensions.BuildPackage(stream, Model, packageMetadata);
      var package = DacPackage.Load(stream);
      var options = new PublishOptions();
      var result = service.Publish(package, connectionStringBuilder.InitialCatalog, options);
      WriteObject(result);
    }
  }
}

