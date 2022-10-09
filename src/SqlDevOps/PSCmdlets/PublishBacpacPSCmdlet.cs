using Microsoft.SqlServer.Dac;

using SqlDevOps.DacFx;
using SqlDevOps.PSCmdlets.BasePSCmdlets;
using SqlDevOps.Utilities;
using System.IO;
using System.Management.Automation;

namespace SqlDevOps.PSCmdlets
{
  [Cmdlet(VerbsData.Publish, PSCmdletNouns.Bacpac)]
  public class PublishBacpacPSCmdlet : BaseDbConnectionPSCmdlet
  {

    #region Parameters

    [Parameter(
      Mandatory = true,
      Position = 0)]
    public string? Path { get; set; }

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

      WriteVerbose($"Publishing '{normalizedPath}' to '[{connectionStringBuilder.DataSource}].[{connectionStringBuilder.InitialCatalog}]'.");
      var dacService = new DacServices(connectionStringBuilder.ToString());
      using (var fileStream = File.OpenRead(normalizedPath))
      {
        var bacpac = BacPackage.Load(fileStream);

        var dacDeployOptions = new DacDeployOptions()
        {
          AllowIncompatiblePlatform = true,
          BlockWhenDriftDetected = false,
          RegisterDataTierApplication = false,
          ScriptNewConstraintValidation = false,
          AllowDropBlockingAssemblies = true,
          DropObjectsNotInSource = true,
          DropPermissionsNotInSource = true,
          DropStatisticsNotInSource = true,
          DropRoleMembersNotInSource = true,
          DoNotDropDatabaseWorkloadGroups = false,
          DoNotDropWorkloadClassifiers = false,
          IgnoreAnsiNulls = false,
          IgnoreKeywordCasing = false,
          IgnoreQuotedIdentifiers = false,
          IgnoreSemicolonBetweenStatements = false,
          IgnoreWhitespace = false,
          ScriptDatabaseCompatibility = true,
        };

        dacService.MyImportBacpac(bacpac, connectionStringBuilder.InitialCatalog, dacDeployOptions, CancellationTokenSource.Token);
      }
    }
  }
}
