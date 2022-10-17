using Microsoft.SqlServer.Dac.Model;
using SqlDevOps.PSCmdlets.BasePSCmdlets;
using SqlDevOps.Extensions;
using System.Management.Automation;
using SqlDevOps.Utilities;

namespace SqlDevOps.PSCmdlets
{
  [Cmdlet(VerbsCommon.Remove, PSCmdletNouns.TSqlObject, SupportsShouldProcess = true)]
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

    [Parameter()]
    public SwitchParameter RemoveReferencingObjects { get; set; }

    #endregion Parameters

    // TODO: Deleting a table should remove foreign keys and indexes.
    protected override void ProcessRecord()
    {
      foreach (var obj in TSqlObject!)
      {
        var nodes =
          RemoveReferencingObjects
          ? obj.GetReferencingPostOrderTraversal()
          : new TreeNode<TSqlObject>[] { new TreeNode<TSqlObject>(0, obj) };

        foreach (var node in nodes)
        {
          if (ShouldProcess(node.Value.ToCliString(), "Remove"))
          {
            WriteVerbose($"Deleting '{node.Value.ToCliString()}'...");
            Model!.DeleteObject(node.Value);
          }
        }
      }
    }

    protected override void EndProcessing()
    {
      // TODO: do not return anything if WhatIf is present.
      WriteObject(Model);
    }
  }
}
