using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Threading;

using Microsoft.Data.SqlClient;
using Microsoft.Data.Tools.Schema;
using Microsoft.Data.Tools.Schema.Common;
using Microsoft.Data.Tools.Schema.Common.SqlClient;
using Microsoft.Data.Tools.Schema.SchemaModel;
using Microsoft.Data.Tools.Schema.Sql;
using Microsoft.Data.Tools.Schema.Sql.Build;
using Microsoft.Data.Tools.Schema.Sql.Common;
using Microsoft.Data.Tools.Schema.Sql.Dac;
using Microsoft.Data.Tools.Schema.Sql.Dac.Data.Export;
using Microsoft.Data.Tools.Schema.Sql.Dac.Data;
using Microsoft.Data.Tools.Schema.Sql.Deployment;
using Microsoft.Data.Tools.Schema.Sql.SchemaModel;
using Microsoft.SqlServer.Dac;
using Microsoft.SqlServer.Dac.Model;

using ReflectionMagic;

namespace SqlDevOps.DacFx
{
  public static class DacServicesExtensions
  {
    // TODO: I don't really like this. Spit into two different private member extension methods.
    internal static DataSchemaModel GetDataSchemaModel(this TSqlModel model)
    {
      SqlSchemaModelObjectService service = model.AsDynamic()._service;

      DataSchemaModel dataSchemaModel = service.AsDynamic()._model;

      return dataSchemaModel;
    }

    internal static (List<DacMessage>, SqlPlatforms) ValidateModelForExport(
      this DacServices dacServices,
      object operation,
      DataSchemaModel dataSchemaModel,
      IList<SqlTable> tables,
      DacLoggingContext dacLoggingContext,
      CancellationToken cancellationToken,
      EngineVersion targetEngineVersion,
      ErrorManager errorManager)
    {
      SqlPlatforms azurePlatformSurfaceArea;

      List<DacMessage> validationMessages = dacServices.AsDynamic().ValidateModelForExport(operation, dataSchemaModel, tables, dacLoggingContext, cancellationToken, targetEngineVersion, errorManager, out azurePlatformSurfaceArea);

      return (validationMessages, azurePlatformSurfaceArea);
    }

    public static void MyImportBacpac(
      this DacServices dacServices,
      BacPackage package,
      string targetDatabaseName,
      DacDeployOptions dacDeployOptions,
      CancellationToken cancellationToken)
    {
      ArgumentValidation.CheckForNullReference(package, "package");
      ArgumentValidation.CheckForEmptyString(targetDatabaseName, "targetDatabaseName");
      cancellationToken.ThrowIfCancellationRequested();

      dacDeployOptions.ImportingBacpac = true;
      dacDeployOptions.IgnoreUserSettingsObjects = true;
      dacDeployOptions.IgnoreLoginSids = true;
      dacDeployOptions.IgnorePermissions = true;
      dacDeployOptions.IgnoreRoleMembership = true;

      DacLoggingContext loggingContext = dacServices.CreateLoggingContext(DacTask.ImportData);
      dacDeployOptions.EnableDnrOrdering(enabled: true);
      IStackSettingsContext stackSettingsContext = null;
      IStackLogSettingsContext stackLogSettingsContext = null;

      try
      {
        dacServices.SetAmbientSetting("SupportAlwaysEncrypted", true);
        stackSettingsContext = dacServices.CreateSettingsContext(loggingContext);
        stackLogSettingsContext = dacServices.CreateLogSettingsContext(dacDeployOptions.HashObjectNamesInLogs);
        dacServices.InternalDeploy(package.PackageSource, isDacpac: false, targetDatabaseName, dacDeployOptions, cancellationToken, loggingContext);
      }
      finally
      {
        stackSettingsContext?.Dispose();
        stackLogSettingsContext?.Dispose();
      }
    }

    public static void MyExportBacpac(
      this DacServices dacServices,
      string path,
      string databaseName,
      TSqlModel model,
      IEnumerable<Tuple<string, string>> tables,
      CancellationToken cancellationToken,
      EngineVersion targetEngineVersion,
      bool verifyFullTextDocumentTypesSupported = false,
      bool ignoreIndexesStatisticsOnEnclaveEnabledColumns = false,
      string tempDirectoryForTableData = null)
    {
      Func<Stream> streamGetter = () => dacServices.GetSafeFileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, DacErrorResources.ErrorSavingPackage);

      SqlConnectionFactory sqlConnectionFactory = dacServices.GetSqlConnectionFactory();

      var databaseSchemaProvider = DacServices.GetDatabaseSchemaProvider(sqlConnectionFactory, databaseName);

      if (databaseSchemaProvider is SqlDwDatabaseSchemaProvider)
      {
        throw ExceptionHelper.CreateServiceException(DacErrorResources.ErrorUnsupportedPlatformForDataPackage);
      }

      // TODO: What is this for?
      if (targetEngineVersion == EngineVersion.V11)
      {
        verifyFullTextDocumentTypesSupported = false;
      }

      DacMetadata metadata = new DacMetadata(databaseName, "0.0.0.0", null);
      DacLoggingContext exportloggingContext = dacServices.CreateLoggingContext(DacTask.ExportData);
      IStackLogSettingsContext stackLogSettingsContext = null;
      IStackSettingsContext stackSettingsContext = null;
      uint? minModelVersion = null;
      var dataSchemaModel = model.GetDataSchemaModel();

      try
      {
        dacServices.SetAmbientSetting("SupportAlwaysEncrypted", true);
        stackSettingsContext = dacServices.CreateSettingsContext(exportloggingContext);

        OperationLogger logger = new OperationLogger(73113, 73114, 73116, DeploymentResources.ServiceActionExtractVerify, exportloggingContext);
        // TODO: What is a better name for this operation? It looks like it is related to validation (most of which I have commented out for now). Also, move this to it's own method.
        IOperation second = new Operation(OperationResources.BacpacModelValidationCaption, (operation, cancellationToken) =>
        {
          logger.Capture(() =>
          {
            DataSchemaModel schemaModel = dataSchemaModel;
            IList<SqlTable> list = DacServices.LookupTableElements<SqlTable>(tables, schemaModel).ToList();
            IList<SqlTableBase> tableElements = new List<SqlTableBase>(list);
            ModelValidation.CheckForSecurityPolicy(schemaModel, tableElements, exportloggingContext);
            ModelValidation.CheckForUnmaskPermissionsNeeded(schemaModel, tableElements, exportloggingContext);
            dacServices.ExcludeRandomizedIndexesFromBacpac(schemaModel, list, ignoreIndexesStatisticsOnEnclaveEnabledColumns, exportloggingContext);
            ModelValidation.CheckForLedgerWarnings(schemaModel, list, exportloggingContext, dacServices);
            SqlPlatforms azurePlatformSurfaceArea;
            // TODO:
            //(List<DacMessage> list2, azurePlatformSurfaceArea) = dacServices.ValidateModelForExport(operation, schemaModel, list, exportloggingContext, cancellationToken, targetEngineVersion, extractOperation.ErrorManager);
            (List<DacMessage> list2, azurePlatformSurfaceArea) = dacServices.ValidateModelForExport(operation, schemaModel, list, exportloggingContext, cancellationToken, targetEngineVersion, new ErrorManager());
            if (list2.Count > 0)
            {
              // TODO:
              //throw ExceptionHelper.CreateServiceException(DacErrorResources.ErrorFoundUnsupportedElementsForDataPackage, list2);
            }
            if (azurePlatformSurfaceArea == SqlPlatforms.SqlAzureV12)
            {
              IList<SqlFilegroup> elements = schemaModel.GetElements<SqlFilegroup>(ModelElementQueryFilter.Internal);
              minModelVersion = elements?.Count > 0 ? 280u : 260u;
            }
          });
        });

        Func<uint?> getMinModelVersion = () => minModelVersion;

        IOperation exportOperation = dacServices.MyCreateExportOperation(streamGetter, databaseName, metadata, tables, dataSchemaModel, DateTime.Now, false, getMinModelVersion, tempDirectoryForTableData, exportloggingContext, CompressionOption.Normal);
        second.Combine(exportOperation).Execute(exportloggingContext, cancellationToken);
      }
      finally
      {
        stackSettingsContext?.Dispose();
        dacServices.ClearLogSettings();
        stackLogSettingsContext?.Dispose();
        exportloggingContext.WaitForAllMessages();
      }
    }

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
      dacExtractOptions.ExtractApplicationScopedObjectsOnly = true;
      dacExtractOptions.EnforceSqlAzureRestrictions = false;

      Func<Stream> streamGetter = () => dacServices.GetSafeFileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, DacErrorResources.ErrorSavingPackage);

      SqlConnectionFactory sqlConnectionFactory = dacServices.GetSqlConnectionFactory();

      var databaseSchemaProvider = DacServices.GetDatabaseSchemaProvider(sqlConnectionFactory, databaseName);

      if (databaseSchemaProvider is SqlDwDatabaseSchemaProvider)
      {
        throw ExceptionHelper.CreateServiceException(DacErrorResources.ErrorUnsupportedPlatformForDataPackage);
      }

      if (targetEngineVersion == EngineVersion.V11)
      {
        verifyFullTextDocumentTypesSupported = false;
      }

      DacMetadata metadata = new DacMetadata(databaseName, "0.0.0.0", null);
      DacLoggingContext exportloggingContext = dacServices.CreateLoggingContext(DacTask.ExportData);
      IStackLogSettingsContext stackLogSettingsContext = null;
      IStackSettingsContext stackSettingsContext = null;
      ExtractOperation extractOperation = null;
      uint? minModelVersion = null;

      int databaseLockTimeInMs = dacServices.GetDatabaseLockTimeInMS(dacExtractOptions.DatabaseLockTimeout);

      // TODO:
      dacExtractOptions.IgnoreUserLoginMappings = true;

      try
      {
        dacServices.SetAmbientSetting("QueryTimeout", dacExtractOptions.CommandTimeout);
        dacServices.SetAmbientSetting("LongRunningQueryTimeout", dacExtractOptions.LongRunningCommandTimeout);
        dacServices.SetAmbientSetting("DatabaseLockTimeout", databaseLockTimeInMs);
        dacServices.SetAmbientSetting("SupportAlwaysEncrypted", true);
        stackSettingsContext = dacServices.CreateSettingsContext(exportloggingContext);
        stackLogSettingsContext = dacServices.CreateLogSettingsContext(dacExtractOptions.HashObjectNamesInLogs);

        extractOperation =
          new ExtractOperation(
            sqlConnectionFactory,
            databaseName,
            dacExtractOptions,
            exportloggingContext,
            includeIsNotTrustedPropertyAnnotations: true,
            promoteDnrWarningsToErrors: false,
            throwOnValidationErrors: false,
            verifyFullTextDocumentTypesSupported,
            storeSourceCodePositionAnnotations: false,
            leaveModelOpen: false,
            ignoreIndexesStatisticsOnEnclaveEnabledColumns);

        OperationLogger logger = new OperationLogger(73113, 73114, 73116, DeploymentResources.ServiceActionExtractVerify, exportloggingContext);
        IOperation second = new Operation(OperationResources.BacpacModelValidationCaption, (operation, cancellationToken) =>
        {
          logger.Capture(() =>
          {
            DataSchemaModel schemaModel = extractOperation.SchemaModel;
            IList<SqlTable> list = DacServices.LookupTableElements<SqlTable>(tables, schemaModel).ToList();
            IList<SqlTableBase> tableElements = new List<SqlTableBase>(list);
            ModelValidation.CheckForSecurityPolicy(schemaModel, tableElements, exportloggingContext);
            ModelValidation.CheckForUnmaskPermissionsNeeded(schemaModel, tableElements, exportloggingContext);
            dacServices.ExcludeRandomizedIndexesFromBacpac(schemaModel, list, ignoreIndexesStatisticsOnEnclaveEnabledColumns, exportloggingContext);
            ModelValidation.CheckForLedgerWarnings(schemaModel, list, exportloggingContext, dacServices);
            SqlPlatforms azurePlatformSurfaceArea;
            (List<DacMessage> list2, azurePlatformSurfaceArea) = dacServices.ValidateModelForExport(operation, schemaModel, list, exportloggingContext, cancellationToken, targetEngineVersion, extractOperation.ErrorManager);
            if (list2.Count > 0)
            {
              // TODO:
              //throw ExceptionHelper.CreateServiceException(DacErrorResources.ErrorFoundUnsupportedElementsForDataPackage, list2);
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

        IOperation second2 = dacServices.CreateExportOperation(streamGetter, databaseName, metadata, tables, cancellationToken, extractOperation, DateTime.Now, false, getMinModelVersion, tempDirectoryForTableData, exportloggingContext, dacExtractOptions.CompressionOption);
        extractOperation.Combine(second).Combine(second2).Execute(exportloggingContext, cancellationToken);
      }
      finally
      {
        stackSettingsContext?.Dispose();
        dacServices.ClearLogSettings();
        stackLogSettingsContext?.Dispose();
        if (extractOperation != null)
        {
          extractOperation.Dispose();
        }
        exportloggingContext.WaitForAllMessages();
      }
    }

    // TODO: Should this take a CancellationToken
    private static Operation MyCreateExportOperation(
      this DacServices dacServices,
      Func<Stream> streamGetter,
      string databaseName, 
      DacMetadata metadata,
      IEnumerable<Tuple<string, string>> tables,
      DataSchemaModel dataSchemaModel,
      DateTime operationStartTime,
      bool isDacpac,
      Func<uint?> getMinModelVersion,
      string temporaryDirectory,
      DacLoggingContext loggingContext,
      CompressionOption compressionOption)
    {
      var logger = new OperationLogger(73129, 73130, 73132, DeploymentResources.ServiceActionExtractData, loggingContext);
      return
        new Operation(
          OperationResources.ExportCaption,
          (operation, token) =>
          {
            logger.Capture(() =>
              {
                try
                {
                  var list = DacServices.LookupTableElements<SqlTable>(tables, dataSchemaModel).Select(x => new ExportTableMetadata(x)).ToList();

                  using Stream stream = streamGetter();
                  using Package package = Package.Open(stream, FileMode.Create, FileAccess.Write);

                  _ = stream.Position;
                  TemporaryStorageProvider storageProvider = new TemporaryStorageProvider(temporaryDirectory);

                  uint num = Math.Max(getMinModelVersion().GetValueOrDefault(0), 250u);

                  uint modelVersion = 0u;
                  byte[] checksum = null;
                  Microsoft.SqlServer.Dac.DacPackage.Save(package, dataSchemaModel, metadata, out modelVersion, out checksum, num);

                  using (BacpacPackage bacpacPackage = BacpacPackage.OpenForWrite(package))
                  {
                    bacpacPackage.CompressionOption = compressionOption;
                    new ExportBacpacStep(dacServices.GetSqlConnectionFactory(), databaseName, bacpacPackage, list, storageProvider, token, loggingContext).Execute();
                    bacpacPackage.AddTableToFilePartRelationships();
                  }

                  DacOrigin dacOrigin = new DacOrigin(System.Reflection.Assembly.GetExecutingAssembly())
                  {
                    OperationStartTime = operationStartTime,
                    ContainsExportedData = !isDacpac
                  };

                  string databaseVersion = DatabaseInfoRetriever.GetDatabaseVersion(dacServices.GetSqlConnectionFactory());

                  if (!string.IsNullOrEmpty(databaseVersion))
                  {
                    // TODO:
                    //throw new NotImplementedException("Whoops");
                    //dacOrigin.SetServerInfo(databaseVersion, dataSchemaModel);
                  }

                  if (tables?.Count() > 0 && DatabaseInfoRetriever.TryGetDatabaseSizeAndRowCount(dacServices.GetSqlConnectionFactory(), databaseName, tables, out var databaseSize, out var rowCount))
                    dacOrigin.SetExportInfo(databaseSize, rowCount);

                  dacOrigin.PackageVersion = DacUtilities.CalculatePackageVersion(modelVersion, hasData: true, dacOrigin.HasPublicDeploymentContributors);
                  dacOrigin.ModelSchemaVersion = ((double)modelVersion / 100.0).ToString("n1", CultureInfo.InvariantCulture);
                  dacOrigin.AddChecksum("/model.xml", checksum);
                  Uri partUri = PackUriHelper.CreatePartUri(new Uri("Origin.xml", UriKind.Relative));
                  using Stream stream2 = package.CreatePart(partUri, "text/xml", CompressionOption.Normal).GetStream(FileMode.OpenOrCreate, FileAccess.Write);
                  dacOrigin.Write(stream2);
                }
                catch (DacServicesException)
                {
                  throw;
                }
                catch (DacException ex)
                {
                  throw ExceptionHelper.CreateServiceException(DacErrorResources.ErrorExportingPackage, ex);
                }
                catch (SqlException ex)
                {
                  throw ExceptionHelper.CreateServiceException(DacErrorResources.ErrorExportingPackage, ex);
                }
                catch (InvalidOperationException ex)
                {
                  throw ExceptionHelper.CreateServiceException(DacErrorResources.ErrorExportingPackage, ex);
                }
                catch (AggregateException ex)
                {
                  throw ExceptionHelper.CreateServiceException(DacErrorResources.ErrorExportingPackage, ex);
                }
              });
          });
    }
  }
}
