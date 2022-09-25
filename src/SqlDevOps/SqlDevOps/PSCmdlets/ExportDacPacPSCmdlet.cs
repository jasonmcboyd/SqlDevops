using Microsoft.SqlServer.Dac;
using Microsoft.SqlServer.Dac.Model;
using SqlDevOps.PSCmdlets.BasePSCmdlets;
using System.IO;
using System.Management.Automation;

namespace SqlDevOps.PSCmdlets
{
  [Cmdlet(VerbsData.Export, PSCmdletNouns.DacPac)]
  [OutputType(typeof(FileInfo))]
  public class ExportDacPacPSCmdlet : BaseSqlDevOpsPSCmdlet
  {

    #region Parameters

    [Parameter(
      Mandatory = true,
      Position = 0)]
    public string? Path { get; set; }

    [Parameter(
      Mandatory = true,
      Position = 1,
      ValueFromPipeline = true)]
    public TSqlModel? Model { get; set; }

    // TODO: Does this work?
    [Parameter()]
    public SwitchParameter IgnoreValidationErrors { get; set; }

    [Parameter()]
    public SwitchParameter Overwrite { get; set; }

    #endregion Parameters

    protected override void ProcessRecord()
    {
      if (Path is null) throw new PSArgumentNullException(nameof(Path));

      var normalizedPath = NormalizePath(Path);
      if (File.Exists(normalizedPath))
      {
        if (Overwrite)
        {
          WriteVerbose($"'{normalizedPath}' exists but {nameof(Overwrite)} switch was included.");
        }
        else
        {
          throw new PSInvalidOperationException($"'{normalizedPath}' already exists. Use {nameof(Overwrite)} switch if intending to overwrite the existing file.");
        }
      }
      var packageOptions = new PackageOptions();
      // TODO: Figure out _exactly_ how IgnoreValidationErrors works.
      if (IgnoreValidationErrors)
      {
        packageOptions.IgnoreValidationErrors = new string[] { "*" };
      }
      var packageMetadata = new PackageMetadata();
      DacPackageExtensions.BuildPackage(normalizedPath, Model, packageMetadata, packageOptions);
      WriteObject(new FileInfo(normalizedPath));
    }

  }
}
