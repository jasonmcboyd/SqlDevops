using Microsoft.SqlServer.Dac.Model;
using SqlDevOps.PSCmdlets.BasePSCmdlets;
using SqlDevOps.Utilities;
using System.Management.Automation;
using SqlDevOps.Extensions;
using System;
using System.Reflection;

namespace SqlDevOps.PSCmdlets
{
  [Cmdlet(VerbsCommon.Get, PSCmdletNouns.TSqlObject)]
  [OutputType(typeof(TSqlObject))]
  public class GetTSqlObjectPSCmdlet : BaseSqlDevOpsPSCmdlet
  {
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
      nameof(Login),
      nameof(RoleMembership),
      nameof(Procedure),
      nameof(Table),
      nameof(User))]
    public string[]? ObjectType { get; set; }

    #endregion Parameters

    private ModelTypeClass[]? TypeClasses { get; set; }

    protected override void BeginProcessing()
    {
      base.BeginProcessing();

      if (ObjectType == null || ObjectType.Length == 0)
        return;

      var exampleModelType = typeof(RoleMembership);
      var propertyName = nameof(RoleMembership.TypeClass);
      var modelNamespace = exampleModelType.Namespace;
      var assembly = System.Reflection.Assembly.GetAssembly(exampleModelType);

      TypeClasses =
        ObjectType
        .Select(x => $"{modelNamespace}.{x}")
        .Select(assembly.GetType)
        .Select(x => x.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Static))
        .Select(x => x.GetValue(null, null))
        .Cast<ModelTypeClass>()
        .ToArray();
    }

    protected override void ProcessRecord()
    {
      var model = Model!;

      if (TypeClasses == null || TypeClasses.Length == 0)
      {
        model.GetObjects(DacQueryScopes.All).ForEach(WriteObject);
        return;
      }

      TypeClasses
        .SelectMany(typeClass => model.GetObjects(DacQueryScopes.UserDefined, typeClass))
        .ForEach(WriteObject);
    }
  }
}
