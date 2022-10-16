using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Threading;

using Microsoft.Data.Tools.Schema;
using Microsoft.Data.Tools.Schema.Common;
using Microsoft.Data.Tools.Schema.Common.SqlClient;
using Microsoft.Data.Tools.Schema.SchemaModel;
using Microsoft.Data.Tools.Schema.Sql;
using Microsoft.Data.Tools.Schema.Sql.Common;
using Microsoft.Data.Tools.Schema.Sql.Dac;
using Microsoft.Data.Tools.Schema.Sql.Deployment;
using Microsoft.Data.Tools.Schema.Sql.SchemaModel;
using Microsoft.SqlServer.Dac;

using ReflectionMagic;
using System.Reflection;

namespace SqlDevOps.DacFx
{
  internal static class DacServicesPrivateMembersExtensions
  {

    internal static Stream GetSafeFileStream(
      this DacServices dacServices,
      string path,
      FileMode fileMode,
      FileAccess fileAccess,
      FileShare fileShare,
      string dacErrorResources) => dacServices.AsDynamic().SafeFileStreamGetter(path, fileMode, fileAccess, fileShare, dacErrorResources);

    internal static SqlConnectionFactory GetSqlConnectionFactory(this DacServices dacServices) => dacServices.AsDynamic()._userConnectionFactory;

    internal static DacLoggingContext CreateLoggingContext(this DacServices dacServices, DacTask dacTask) => dacServices.AsDynamic().CreateLoggingContext(dacTask);

    internal static int GetDatabaseLockTimeInMS(this DacServices dacServices, int databaseLockTimeout) => dacServices.AsDynamic().GetDatabaseLocktimeinMS(databaseLockTimeout);

    internal static IStackLogSettingsContext CreateLogSettingsContext(this DacServices dacServices, bool hashObjectNamesInLogs) => dacServices.AsDynamic().CreateLogSettingsContext(hashObjectNamesInLogs);

    internal static IStackSettingsContext CreateSettingsContext(this DacServices dacServices, DacLoggingContext dacLoggingContext) => dacServices.AsDynamic().CreateSettingsContext(dacLoggingContext);

    internal static void ClearLogSettings(this DacServices dacServices) => dacServices.AsDynamic().ClearLogSettings();

    internal static void ExcludeRandomizedIndexesFromBacpac(
      this DacServices dacServices,
      DataSchemaModel dataSchemaModel,
      IList<SqlTable> tables,
      bool ignoreIndexesStatisticsOnEnclaveEnabledColumns,
      DacLoggingContext dacLoggingContext)
      => dacServices.AsDynamic().ExcludeRandomizedIndexesFromBacpac(dataSchemaModel, tables, ignoreIndexesStatisticsOnEnclaveEnabledColumns, dacLoggingContext);

    internal static IOperation CreateExportOperation(
      this DacServices dacServices,
      Func<Stream> streamGetter,
      string databaseName,
      DacMetadata dacMetadata,
      IEnumerable<Tuple<string, string>> tables,
      CancellationToken cancellationToken,
      ExtractOperation extractOperation,
      DateTime operationStartTime,
      bool isDacpac,
      Func<uint?> getMinModelVersion,
      string temporaryDirectory,
      DacLoggingContext dacLoggingContext,
      CompressionOption compressionOption)
      => dacServices.AsDynamic().CreateExportOperation(streamGetter, databaseName, dacMetadata, tables, cancellationToken, extractOperation, operationStartTime, false, getMinModelVersion, temporaryDirectory, dacLoggingContext, compressionOption);

    internal static void InternalDeploy(
      this DacServices dacServices,
      IPackageSource packageSource,
      bool isDacpac,
      string targetDatabaseName,
      DacDeployOptions options,
      CancellationToken cancellationToken,
      DacLoggingContext loggingContext,
      Action<IDeploymentController, DeploymentPlan, ErrorManager> reportPlanOperation = null,
      bool executePlan = true)
      => dacServices.AsDynamic().InternalDeploy(packageSource, isDacpac, targetDatabaseName, options, cancellationToken, loggingContext, reportPlanOperation, executePlan);
  }
}
