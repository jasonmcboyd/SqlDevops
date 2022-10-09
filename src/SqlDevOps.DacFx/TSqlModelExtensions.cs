using Microsoft.Data.Tools.Schema.SchemaModel;
using Microsoft.SqlServer.Dac.Data.Model;

using ReflectionMagic;

namespace SqlDevOps.DacFx
{
    internal static class TSqlModelExtensions
    {
      internal static DataSchemaModel GetDataSchemaModel(this TSqlModel model) => model.AsDynamic()._dataSchemaModel;
    }
}
