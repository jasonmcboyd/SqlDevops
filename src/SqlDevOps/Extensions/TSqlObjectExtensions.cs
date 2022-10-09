using Microsoft.SqlServer.Dac.Model;

namespace SqlDevOps.Extensions
{
  public static class TSqlObjectExtensions
  {
    public static string GetSchema(this TSqlObject sqlObject) => sqlObject.Name.Parts[0].Replace(@"\", "_");
    public static string GetObjectType(this TSqlObject sqlObject) => sqlObject.ObjectType.Name;
    public static string GetFilename(this TSqlObject sqlObject) => $"{sqlObject.Name.Parts[^1].Replace(@"\", "_")}.sql";
  }
}
