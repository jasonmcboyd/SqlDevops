using Microsoft.SqlServer.Dac.Model;
using SqlDevOps.PSCmdlets.BasePSCmdlets;
using SqlDevOps.Extensions;
using SqlDevOps.Utilities;
using System.Management.Automation;

namespace SqlDevOps.PSCmdlets
{
  [Cmdlet(VerbsCommon.Remove, PSCmdletNouns.TSqlObject)]
  public class RemoveTSqlObjectPSCmdlet : BaseSqlDevOpsPSCmdlet
  {
    #region Parameters

    [Parameter(
      Mandatory = true,
      Position = 0)]
    public TSqlModel Model { get; set; }

    [Parameter(
      Mandatory = true,
      Position = 1,
      ValueFromPipeline = true,
      ValueFromPipelineByPropertyName = true)]
    public TSqlObject[] TSqlObject { get; set; }

    #endregion Parameters

    protected override void ProcessRecord() => TSqlObject.ForEach(Model.DeleteObject);
  }
}
