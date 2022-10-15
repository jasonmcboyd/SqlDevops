using Microsoft.SqlServer.Dac.Model;
using System.Collections.Generic;

namespace SqlDevOps.Extensions
{
  public static class TSqlModelExtensions
  {
    public static IEnumerable<TSqlObject> GetStoredProcedures(this TSqlModel model) => model.GetObjects(DacQueryScopes.UserDefined, Procedure.TypeClass);

    public static void DeleteObject(this TSqlModel model, TSqlObject obj) => model.DeleteObjects(obj.GetSourceInformation().SourceName);
  }
}
