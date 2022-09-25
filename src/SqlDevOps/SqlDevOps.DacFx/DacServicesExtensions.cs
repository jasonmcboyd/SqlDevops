using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
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

namespace SqlDevOps.DacFx
{
  public static class DacServicesExtensions
  {
    public static void MyExportBacpac(
      this DacServices dacServices,
      string path,
      string databaseName,
      DacExtractOptions dacExtractOptions,
      IEnumerable<Tuple<string, string>> tables,
      CancellationToken cancellationToken,
      EngineVersion targetEngineVersion = EngineVersion.Latest,
      bool verifyFullTextDocumentTypesSupported = false,
      bool ignoreIndexesStatisticsOnEnclaveEnabledColumns = false,
      string tempDirectoryForTableData = null)
    {
      var test = new SqlDwDatabaseSchemaProvider();
      var dynamicDacServices = dacServices.AsDynamic();

      Func<Stream> streamGetter = () => dynamicDacServices.SafeFileStreamGetter(path, FileMode.Create, FileAccess.Write, FileShare.None, DacErrorResources.ErrorSavingPackage);

      SqlConnectionFactory sqlConnectionFactory = dynamicDacServices._userConnectionFactory;

      var databaseSchemaProvider = DacServices.GetDatabaseSchemaProvider(sqlConnectionFactory, databaseName);

      if (databaseSchemaProvider is SqlDwDatabaseSchemaProvider)
      {
        throw ExceptionHelper.CreateServiceException(DacErrorResources.ErrorUnsupportedPlatformForDataPackage);
      }

      if (targetEngineVersion == EngineVersion.V11)
      {
        verifyFullTextDocumentTypesSupported = false;
      }

      DateTime now = DateTime.Now;
      DacMetadata metadata = new DacMetadata(databaseName, "0.0.0.0", null);
      DacLoggingContext exportloggingContext = dynamicDacServices.CreateLoggingContext(DacTask.ExportData);
      IStackLogSettingsContext stackLogSettingsContext = null;
      IStackSettingsContext stackSettingsContext = null;
      ExtractOperation extractOperation = null;
      uint? minModelVersion = null;

      int databaseLockTimeInMs = dynamicDacServices.GetDatabaseLocktimeinMS(dacExtractOptions.DatabaseLockTimeout);

      try
      {
        dacServices.SetAmbientSetting("QueryTimeout", dacExtractOptions.CommandTimeout);
        dacServices.SetAmbientSetting("LongRunningQueryTimeout", dacExtractOptions.LongRunningCommandTimeout);
        dacServices.SetAmbientSetting("DatabaseLockTimeout", databaseLockTimeInMs);
        dacServices.SetAmbientSetting("SupportAlwaysEncrypted", true);
        stackSettingsContext = dynamicDacServices.CreateSettingsContext(exportloggingContext);
        stackLogSettingsContext = dynamicDacServices.CreateLogSettingsContext(dacExtractOptions.HashObjectNamesInLogs);
        extractOperation = new ExtractOperation(sqlConnectionFactory, databaseName, dacExtractOptions, exportloggingContext, includeIsNotTrustedPropertyAnnotations: true, promoteDnrWarningsToErrors: false, throwOnValidationErrors: false, verifyFullTextDocumentTypesSupported, storeSourceCodePositionAnnotations: false, leaveModelOpen: false, ignoreIndexesStatisticsOnEnclaveEnabledColumns);
        OperationLogger logger = new OperationLogger(73113, 73114, 73116, DeploymentResources.ServiceActionExtractVerify, exportloggingContext);
        IOperation second = new Operation(OperationResources.BacpacModelValidationCaption, delegate (object operation, CancellationToken token)
        {
          logger.Capture(delegate
          {
            DataSchemaModel schemaModel = extractOperation.SchemaModel;
            IList<SqlTable> list = DacServices.LookupTableElements<SqlTable>(tables, schemaModel).ToList();
            IList<SqlTableBase> tableElements = new List<SqlTableBase>(list);
            ModelValidation.CheckForSecurityPolicy(schemaModel, tableElements, exportloggingContext);
            ModelValidation.CheckForUnmaskPermissionsNeeded(schemaModel, tableElements, exportloggingContext);
            dynamicDacServices.ExcludeRandomizedIndexesFromBacpac(schemaModel, list, ignoreIndexesStatisticsOnEnclaveEnabledColumns, exportloggingContext);
            ModelValidation.CheckForLedgerWarnings(schemaModel, list, exportloggingContext, dacServices);
            SqlPlatforms azurePlatformSurfaceArea;
            List<DacMessage> list2 = dynamicDacServices.ValidateModelForExport(operation, schemaModel, list, exportloggingContext, token, targetEngineVersion, extractOperation.ErrorManager, out azurePlatformSurfaceArea);
            if (list2.Count > 0)
            {
              throw ExceptionHelper.CreateServiceException(DacErrorResources.ErrorFoundUnsupportedElementsForDataPackage, list2);
            }
            if (azurePlatformSurfaceArea == SqlPlatforms.SqlAzureV12)
            {
              IList<SqlFilegroup> elements = schemaModel.GetElements<SqlFilegroup>(ModelElementQueryFilter.Internal);
              if (elements != null && elements.Count() > 0)
              {
                minModelVersion = 280u;
              }
              else
              {
                minModelVersion = 260u;
              }
            }
          });
        });

        Func<uint?> getMinModelVersion = () => minModelVersion;

        IOperation second2 = dynamicDacServices.CreateExportOperation(streamGetter, databaseName, metadata, tables, cancellationToken, extractOperation, now, false, getMinModelVersion, tempDirectoryForTableData, exportloggingContext, dacExtractOptions.CompressionOption);
        extractOperation.Combine(second).Combine(second2).Execute(exportloggingContext, cancellationToken);
      }
      finally
      {
        stackSettingsContext?.Dispose();
        dynamicDacServices.ClearLogSettings();
        stackLogSettingsContext?.Dispose();
        if (extractOperation != null)
        {
          extractOperation.Dispose();
        }
        exportloggingContext.WaitForAllMessages();
      }
    }
  }
}
