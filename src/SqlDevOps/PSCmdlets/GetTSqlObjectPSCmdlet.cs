using Microsoft.SqlServer.Dac.Model;
using SqlDevOps.Extensions;
using SqlDevOps.PSCmdlets.BasePSCmdlets;
using System.Collections.Generic;
using System.Management.Automation;
using System.Reflection;
using System.Text.RegularExpressions;

namespace SqlDevOps.PSCmdlets
{
  [Cmdlet(VerbsCommon.Get, PSCmdletNouns.TSqlObject)]
  [OutputType(typeof(TSqlObject))]
  public class GetTSqlObjectPSCmdlet : BaseSqlDevOpsPSCmdlet
  {
    #region Parameters

    protected const string PSN_DEFAULT = $"{nameof(Model)}, {nameof(ObjectType)}";
    protected const string PSN_NAME = $"{nameof(Model)}, {nameof(ObjectType)}, {nameof(Name)}";
    protected const string PSN_LITERAL_NAME = $"{nameof(Model)}, {nameof(ObjectType)}, {nameof(LiteralName)}";

    [Parameter(
      Mandatory = true,
      ParameterSetName = PSN_DEFAULT,
      Position = 0,
      ValueFromPipeline = true,
      ValueFromPipelineByPropertyName = true)]
    [Parameter(
      Mandatory = true,
      ParameterSetName = PSN_LITERAL_NAME,
      Position = 0,
      ValueFromPipeline = true,
      ValueFromPipelineByPropertyName = true)]
    [Parameter(
      Mandatory = true,
      ParameterSetName = PSN_NAME,
      Position = 0,
      ValueFromPipeline = true,
      ValueFromPipelineByPropertyName = true)]
    public TSqlModel? Model { get; set; }

    [Parameter(
      Mandatory = false,
      ParameterSetName = PSN_DEFAULT,
      Position = 1)]
    [Parameter(
      Mandatory = false,
      ParameterSetName = PSN_LITERAL_NAME,
      Position = 1)]
    [Parameter(
      Mandatory = false,
      ParameterSetName = PSN_NAME,
      Position = 1)]
    [ValidateSet(
      nameof(CheckConstraint),
      nameof(DefaultConstraint),
      nameof(ForeignKeyConstraint),
      nameof(Index),
      nameof(Login),
      nameof(PrimaryKeyConstraint),
      nameof(Procedure),
      nameof(Role),
      nameof(RoleMembership),
      nameof(ScalarFunction),
      nameof(Schema),
      nameof(Table),
      nameof(TableValuedFunction),
      nameof(UniqueConstraint),
      nameof(User),
      nameof(View))]
    public string[]? ObjectType { get; set; }

    [Parameter(
      Mandatory = true,
      ParameterSetName = PSN_NAME,
      Position = 2)]
    public string[]? Name { get; set; }

    [Parameter(
      Mandatory = true,
      ParameterSetName = PSN_LITERAL_NAME,
      Position = 2)]
    public string[]? LiteralName { get; set; }

    #endregion Parameters

    private static Dictionary<string, ModelTypeClass> TypeClassMap { get; set; }

    static GetTSqlObjectPSCmdlet()
    {
      var exampleModelType = typeof(RoleMembership);
      var propertyName = nameof(RoleMembership.TypeClass);
      var modelNamespace = exampleModelType.Namespace;
      var assembly = System.Reflection.Assembly.GetAssembly(exampleModelType);

      TypeClassMap =
        assembly!
        .GetTypes()
        .Select(type => type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Static))
        .Where(property => property != null && property.PropertyType == typeof(ModelTypeClass))
        .Cast<PropertyInfo>()
        .ToDictionary(
          property => property.DeclaringType!.Name,
          property => (ModelTypeClass)property.GetValue(null, null)!);
    }

    protected override void ProcessRecord()
    {
      var model = Model!;

      var objects =
        (ObjectType?.Length ?? 0) == 0
        ? model.GetObjects(DacQueryScopes.UserDefined)
        : ObjectType!.Select(type => TypeClassMap![type]).SelectMany(modelTypeClass => model.GetObjects(DacQueryScopes.UserDefined, modelTypeClass));

      switch (ParameterSetName)
      {
        case PSN_LITERAL_NAME:
          objects = objects.Where(obj => LiteralName!.Contains(obj.Name.ToString(), System.StringComparer.OrdinalIgnoreCase));
          break;
        case PSN_NAME:
          objects = objects.Where(obj => Name!.Any(namePattern => Regex.IsMatch(obj.Name.ToString(), namePattern, RegexOptions.IgnoreCase)));
          break;
        case PSN_DEFAULT:
          // Do nothing.
          break;
        default:
          throw new PSNotImplementedException();
      }

      objects.ForEach(WriteObject);
    }
  }
}
