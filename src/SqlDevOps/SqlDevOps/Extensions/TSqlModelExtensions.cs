using Microsoft.SqlServer.Dac.Model;
using System.Collections.Generic;

namespace SqlDevOps.Extensions
{
  public static class TSqlModelExtensions
  {
    public static IEnumerable<TSqlObject> GetLogins(this TSqlModel model) => model.GetObjects(DacQueryScopes.UserDefined, Login.TypeClass);
    public static IEnumerable<TSqlObject> GetStoredProcedures(this TSqlModel model) => model.GetObjects(DacQueryScopes.UserDefined, Procedure.TypeClass);
    public static IEnumerable<TSqlObject> GetTables(this TSqlModel model) => model.GetObjects(DacQueryScopes.UserDefined, Table.TypeClass);
    public static IEnumerable<TSqlObject> GetUsers(this TSqlModel model) => model.GetObjects(DacQueryScopes.UserDefined, User.TypeClass);

    public static void DeleteObject(this TSqlModel model, TSqlObject obj) => model.DeleteObjects(obj.GetSourceInformation().SourceName);
  }
}
