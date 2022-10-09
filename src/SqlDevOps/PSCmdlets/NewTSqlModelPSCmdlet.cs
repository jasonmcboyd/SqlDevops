using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Dac;
using Microsoft.SqlServer.Dac.Model;
using SqlDevOps.PSCmdlets.BasePSCmdlets;
using SqlDevOps.Utilities;
using System.IO;
using System.Management.Automation;

namespace SqlDevOps.PSCmdlets
{
  [Cmdlet(VerbsCommon.New, PSCmdletNouns.TSqlModel)]
  [OutputType(typeof(TSqlModel))]
  public class NewTSqlModelPSCmdlet : BaseDbConnectionPSCmdlet
  {

    #region Parameters

    protected const string PSN_DACPAC = "Dacpac";
    protected const string PSN_SCHEMA_FILES = "SchemaFiles";

    [Parameter(
      Mandatory = true,
      ParameterSetName = PSN_DACPAC,
      Position = 0)]
    public string? DacpacPath { get; set; }

    [Parameter(
      Mandatory = true,
      ParameterSetName = PSN_SCHEMA_FILES,
      Position = 0)]
    public string? SchemaFolderPath { get; set; }

    #endregion Parameters

    protected override void BeginProcessing()
    {
      base.BeginProcessing();
    }

    protected override void EndProcessing()
    {
      base.EndProcessing();
    }


    protected override void ProcessRecord()
    {
      TSqlModel model;

      switch (ParameterSetName)
      {
        case PSN_DACPAC:
          if (DacpacPath is null) throw new PSArgumentNullException(nameof(DacpacPath));
          model = ImportDacpac(DacpacPath);
          break;
        case PSN_SCHEMA_FILES:
          if (SchemaFolderPath is null) throw new PSArgumentNullException(nameof(SchemaFolderPath));
          model = ImportSchemaFiles(SchemaFolderPath);
          break;
        case PSN_AUTHENTICATE_WITH_CONNECTION_STRING:
        case PSN_AUTHENTICATE_WITH_CREDENTIALS:
        case PSN_AUTHENTICATE_WITH_INTEGRATED_SECURITY:
          var connectionStringBuilder =
            SqlConnectionStringBuilderBuilder.Build(
              ConnectionString,
              Server,
              Database,
              Credential,
              IntegratedSecurity);
          model = ImportDatabase(connectionStringBuilder);
          break;
        default:
          // TODO:
          throw new PSNotImplementedException();
      }

      WriteObject(model);
    }

    private TSqlModel ImportDacpac(string path)
    {
      string normalizedPath = NormalizePath(path);
      WriteDebug($"Normalized path: {normalizedPath}");

      var modelLoadOptions = new ModelLoadOptions(DacSchemaModelStorageType.Memory, true);

      using var dacpacStream = File.Open(normalizedPath, FileMode.Open);

      return TSqlModel.LoadFromDacpac(dacpacStream, modelLoadOptions);
    }

    private TSqlModel ImportSchemaFiles(string path)
    {
      string normalizedPath = NormalizePath(path);
      WriteDebug($"Normalized path: {normalizedPath}");

      // TODO: Add support for different versions
      var model = new TSqlModel(SqlServerVersion.Sql150, new TSqlModelOptions());

      foreach (var file in Directory.GetFiles(normalizedPath, "*.sql", SearchOption.AllDirectories))
        model.AddObjects(File.ReadAllText(file));

      return model;
    }

    private TSqlModel ImportDatabase(SqlConnectionStringBuilder connectionStringBuilder)
    {
      // TODO: Consider exposing extract options as parameters
      var modelExtractOptions = new ModelExtractOptions()
      {
        LoadAsScriptBackedModel = true,
        Storage = DacSchemaModelStorageType.Memory
      };
      return TSqlModel.LoadFromDatabase(connectionStringBuilder.ToString(), modelExtractOptions);
    }
  }
}
