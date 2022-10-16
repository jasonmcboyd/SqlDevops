using Microsoft.SqlServer.Dac.Model;
using SqlDevOps.PSCmdlets.BasePSCmdlets;
using SqlDevOps.Extensions;
using System.Management.Automation;

namespace SqlDevOps.PSCmdlets
{
  [Cmdlet(VerbsCommon.Remove, PSCmdletNouns.TSqlObject)]
  [OutputType(typeof(TSqlModel))]
  public class RemoveTSqlObjectPSCmdlet : BaseSqlDevOpsPSCmdlet
  {
    #region Parameters

    [Parameter(
      Mandatory = true,
      Position = 0)]
    public TSqlModel? Model { get; set; }

    [Parameter(
      Mandatory = true,
      Position = 1,
      ValueFromPipeline = true,
      ValueFromPipelineByPropertyName = true)]
    public TSqlObject[]? TSqlObject { get; set; }

    #endregion Parameters

    protected override void ProcessRecord() => TSqlObject!.ForEach(Model!.DeleteObject);

    protected override void EndProcessing() => WriteObject(Model);
  }
}
