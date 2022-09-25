using Microsoft.SqlServer.Dac;
using SqlDevOps.PSCmdlets.BasePSCmdlets;
using SqlDevOps.Utilities;
using System.IO;
using System.Management.Automation;
using SqlDevOps.DacFx;

namespace SqlDevOps.PSCmdlets
{
  [Cmdlet(VerbsData.Export, PSCmdletNouns.Bacpac)]
  [OutputType(typeof(FileInfo))]
  public class ExportBacpacPSCmdlet : BaseDbConnectionPSCmdlet
  {

    #region Parameters

    [Parameter(
      Mandatory = true,
      Position = 0)]
    public string? Path { get; set; }

    [Parameter()]
    public SwitchParameter Overwrite { get; set; }

    // TODO: Does this work?
    [Parameter()]
    public SwitchParameter IgnoreValidationErrors { get; set; }

    #endregion Parameters

    protected override void ProcessRecord()
    {
      if (Path is null)
        throw new PSArgumentNullException(nameof(Path));

      var connectionStringBuilder =
        SqlConnectionStringBuilderBuilder.Build(
          ConnectionString,
          Server,
          Database,
          Credential,
          IntegratedSecurity);

      WriteVerbose($"Exporting bacpac to '{Path}'");
      var service = new DacServices(connectionStringBuilder.ToString());
      using (var fileStream = new FileStream(Path, FileMode.Create))
      {
        var options = new DacExtractOptions()
        {
        };
        //service.ExportBacpac(fileStream, connectionStringBuilder.InitialCatalog, null, CancellationTokenSource.Token);

        service.MyExportBacpac(Path, Database, options, null, CancellationTokenSource.Token);

      }
      WriteObject(new FileInfo(Path));
    }

  }
}
