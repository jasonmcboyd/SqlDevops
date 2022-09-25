using Microsoft.SqlServer.Dac.Model;
using SqlDevOps.PSCmdlets.BasePSCmdlets;
using SqlDevOps.Utilities;
using System.Management.Automation;
using System.Linq;
using SqlDevOps.Extensions;

namespace SqlDevOps.PSCmdlets
{
  [Cmdlet(VerbsCommon.Get, PSCmdletNouns.TSqlObject)]
  [OutputType(typeof(TSqlObject))]
  public class GetTSqlObjectPSCmdlet : BaseSqlDevOpsPSCmdlet
  {

    private static class ObjectTypes
    {
      public const string Login = "Login";
      public const string StoredProcedure = "StoredProcedure";
      public const string Table = "Table";
      public const string User = "User";
    }

    #region Parameters

    [Parameter(
      Mandatory = true,
      Position = 0,
      ValueFromPipeline = true,
      ValueFromPipelineByPropertyName = true)]
    public TSqlModel? Model { get; set; }

    [Parameter(
      Mandatory = false,
      Position = 1)]
    [ValidateSet(
      ObjectTypes.Login,
      ObjectTypes.StoredProcedure,
      ObjectTypes.Table,
      ObjectTypes.User)]
    public string[]? ObjectType { get; set; }

    #endregion Parameters

    protected override void ProcessRecord()
    {
      var model = Model!;

      if (ObjectType == null || ObjectType.Length == 0)
      {
        model.GetObjects(DacQueryScopes.All).ForEach(WriteObject);
        return;
      }

      if (ObjectType.Contains(ObjectTypes.Login))
        model.GetLogins().ForEach(WriteObject);

      if (ObjectType.Contains(ObjectTypes.StoredProcedure))
        model.GetStoredProcedures().ForEach(WriteObject);

      if (ObjectType.Contains(ObjectTypes.Table))
        model.GetTables().ForEach(WriteObject);

      if (ObjectType.Contains(ObjectTypes.User))
        model.GetUsers().ForEach(WriteObject);
    }
  }
}
