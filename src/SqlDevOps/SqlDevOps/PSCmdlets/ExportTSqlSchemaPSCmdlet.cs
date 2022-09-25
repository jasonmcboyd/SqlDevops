using Microsoft.SqlServer.Dac.Model;
using SqlDevOps.PSCmdlets.BasePSCmdlets;
using SqlDevOps.Utilities;
using System.IO;
using System.Linq;
using System.Management.Automation;
using SqlDevOps.Extensions;

namespace SqlDevOps.PSCmdlets
{
  [Cmdlet(VerbsData.Export, PSCmdletNouns.TSqlSchema)]
  public class ExportTSqlSchemaPSCmdlet : BaseSqlDevOpsPSCmdlet
  {

    #region Parameters

    private static class ValidateSet
    {
      internal const string SchemaType = "schema/type";
      internal const string TypeSchema = "type/schema";
    }

    [Parameter(
      Mandatory = true,
      Position = 0)]
    public string? Path { get; set; }

    [Parameter(
      Mandatory = true,
      Position = 1,
      ValueFromPipeline = true)]
    public TSqlModel? Model { get; set; }

    [Parameter(Position = 2)]
    [ValidateSet(ValidateSet.TypeSchema, ValidateSet.SchemaType, IgnoreCase = true)]
    public string? DirectoryStructure { get; set; } = ValidateSet.SchemaType;

    // TODO: Does this work?
    [Parameter()]
    public SwitchParameter IgnoreValidationErrors { get; set; }

    [Parameter()]
    public SwitchParameter Overwrite { get; set; }

    #endregion Parameters

    protected override void ProcessRecord()
    {
      var normalizedPath = NormalizePath(Path);
      var objects = Model.GetObjects(DacQueryScopes.UserDefined).Where(x => x.Name.HasName);

      var dict = objects.ToDictionary(x => ConstructPath(x), x => x);

      if (!Overwrite)
        foreach (var pair in dict)
          if (File.Exists(pair.Key))
            throw new PSInvalidOperationException("This would overwrite a file. Pass the Overwrite switch if overwriting is intentional.");
      
      foreach (var (key, value) in dict)
      {
        var script = value.GetScript();
        Directory.CreateDirectory(System.IO.Path.GetDirectoryName(key));
        File.WriteAllText(key, script);
      }
    }

    private string ConstructPath(TSqlObject obj)
    {
      var path = NormalizePath(Path);

      switch (DirectoryStructure)
      {
        case ValidateSet.SchemaType:
          path = System.IO.Path.Combine(path, obj.GetSchema(), obj.GetObjectType());
          break;
        case ValidateSet.TypeSchema:
          path = System.IO.Path.Combine(path, obj.GetObjectType(), obj.GetSchema());
          break;
        default:
          throw new PSNotImplementedException($"Have not implemented '{DirectoryStructure}' directory structure.");
      }

      path = System.IO.Path.Combine(path, obj.GetFilename());

      return path;
    }

  }
}
