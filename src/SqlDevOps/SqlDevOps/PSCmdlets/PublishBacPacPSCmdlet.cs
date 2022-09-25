using Microsoft.SqlServer.Dac;
using SqlDevOps.PSCmdlets.BasePSCmdlets;
using SqlDevOps.Utilities;
using System.IO;
using System.Management.Automation;

namespace SqlDevOps.PSCmdlets
{
  [Cmdlet(VerbsData.Publish, PSCmdletNouns.BacPac)]
  public class PublishBacPacPSCmdlet : BaseDbConnectionPSCmdlet
  {

    #region Parameters

    [Parameter(
      Mandatory = true,
      Position = 0)]
    public string? Path { get; set; }

    #endregion Parameters

    protected override void ProcessRecord()
    {
      if (Path is null) throw new PSArgumentNullException(nameof(Path));

      var normalizedPath = NormalizePath(Path);

      var connectionStringBuilder =
        SqlConnectionStringBuilderBuilder.Build(
          ConnectionString,
          Server,
          Database,
          Credential,
          IntegratedSecurity);

      WriteVerbose($"Publishing '{normalizedPath}' to '[{connectionStringBuilder.DataSource}].[{connectionStringBuilder.InitialCatalog}]'.");
      var service = new DacServices(connectionStringBuilder.ToString());
      using (var fileStream = File.OpenRead(normalizedPath))
      {
        var bacpac = BacPackage.Load(fileStream);
        service.ImportBacpac(bacpac, connectionStringBuilder.InitialCatalog, CancellationTokenSource.Token);
      }
    }

  }
}
