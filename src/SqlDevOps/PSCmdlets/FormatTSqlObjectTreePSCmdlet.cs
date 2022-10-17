using Microsoft.SqlServer.Dac.Model;

using SqlDevOps.PSCmdlets.BasePSCmdlets;
using SqlDevOps.Utilities;

using System.Collections.Generic;
using System.Management.Automation;
using System.Text;

namespace SqlDevOps.PSCmdlets
{
  [Cmdlet(VerbsCommon.Format, PSCmdletNouns.TSqlObjectTree)]
  [OutputType(typeof(string))]
  public class FormatTSqlObjectTree : BaseSqlDevOpsPSCmdlet
  {
    #region Parameters

    [Parameter(
      Mandatory = true,
      Position = 0,
      ValueFromPipeline = true,
      ValueFromPipelineByPropertyName = true)]
    public TSqlObject[]? TSqlObject { get; set; }

    #endregion Parameters

    private StringBuilder _StringBuilder = new StringBuilder();

    protected override void ProcessRecord()
    {
      var lines = new List<string>();

      foreach (var obj in TSqlObject!)
        foreach (var line in obj.GetReferencingPreOrderTraversal().Select(GetLine))
          lines.Add(line);

      WriteObject(string.Join("\r\n", lines));
    }

    private string GetLine(TreeNode<TSqlObject> node)
    {
      _StringBuilder.Clear();

      if (node.Depth == 0)
        return node.Value.ToCliString();

      for (int i = 0; i < node.Depth; i++)
        _StringBuilder.Append("   ");

      _StringBuilder.Append(node.Value.ToCliString());
      return _StringBuilder.ToString();
    }
  }
}
