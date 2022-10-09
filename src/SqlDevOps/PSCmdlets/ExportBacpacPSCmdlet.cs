using Microsoft.SqlServer.Dac;
using SqlDevOps.PSCmdlets.BasePSCmdlets;
using SqlDevOps.Utilities;
using System.IO;
using System.Management.Automation;
using SqlDevOps.DacFx;
using Microsoft.SqlServer.Dac.Model;

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
    public TSqlModel? Model { get; set; }

    [Parameter()]
    public SwitchParameter Overwrite { get; set; }

    // TODO: Does this work?
    [Parameter()]
    public SwitchParameter IgnoreValidationErrors { get; set; }

    #endregion Parameters

    protected override void ProcessRecord()
    {
      var normalizedPath = NormalizePath(Path!);

      var connectionStringBuilder =
        SqlConnectionStringBuilderBuilder.Build(
          ConnectionString,
          Server,
          Database,
          Credential,
          IntegratedSecurity);

      WriteVerbose($"Exporting bacpac to '{normalizedPath}'");
      var service = new DacServices(connectionStringBuilder.ToString());

      var options = new DacExtractOptions()
      {
        IgnorePermissions = true,
        IgnoreUserLoginMappings = true,
      };

      //var test = new DacExportOptions
      //{
      //  TargetEngineVersion = EngineVersion.V12
      //};

      if (Model is not null)
      {
        service.MyExportBacpac(
          normalizedPath,
          connectionStringBuilder.InitialCatalog,
          Model,
          null,
          CancellationTokenSource.Token,
          targetEngineVersion: EngineVersion.V12);
      }
      else
      {
        service.MyExportBacpac(
          normalizedPath,
          connectionStringBuilder.InitialCatalog,
          options,
          null,
          CancellationTokenSource.Token,
          targetEngineVersion: EngineVersion.V12);
      }

      WriteObject(new FileInfo(normalizedPath));
    }
  }
}
