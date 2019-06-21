# function New-TSqlModelOptionsFromTSqlModel {
#     [CmdletBinding()]
#     param (
#         [parameter(Mandatory=$true, ValueFromPipeline=$true, ValueFromPipelineByPropertyName=$true)]
#         [Microsoft.SqlServer.Dac.Model.TSqlModel]
#         $Model
#     )
    
#     process {
#         $modelOptions = 
#             $Model.GetObjects([Microsoft.SqlServer.Dac.Model.DacQueryScopes]::All, [Microsoft.SqlServer.Dac.Model.DatabaseOptions]::TypeClass) `
#             | Select-Object -First 1

#         $options = [Microsoft.SqlServer.Dac.Model.TSqlModelOptions]::new()

#         $options.Collation                                 = [string]$modelOptions.GetProperty([Microsoft.SqlServer.Dac.Model.DatabaseOptions]::Collation);
#         $options.AllowSnapshotIsolation                    = [bool]$modelOptions.GetProperty([Microsoft.SqlServer.Dac.Model.DatabaseOptions]::AllowSnapshotIsolation);
#         $options.TransactionIsolationReadCommittedSnapshot = [bool]$modelOptions.GetProperty([Microsoft.SqlServer.Dac.Model.DatabaseOptions]::TransactionIsolationReadCommittedSnapshot);
#         $options.AnsiNullDefaultOn                         = [bool]$modelOptions.GetProperty([Microsoft.SqlServer.Dac.Model.DatabaseOptions]::AnsiNullDefaultOn);
#         $options.AnsiNullsOn                               = [bool]$modelOptions.GetProperty([Microsoft.SqlServer.Dac.Model.DatabaseOptions]::AnsiNullsOn);
#         $options.AnsiPaddingOn                             = [bool]$modelOptions.GetProperty([Microsoft.SqlServer.Dac.Model.DatabaseOptions]::AnsiPaddingOn);
#         $options.AnsiWarningsOn                            = [bool]$modelOptions.GetProperty([Microsoft.SqlServer.Dac.Model.DatabaseOptions]::AnsiWarningsOn);
#         $options.ArithAbortOn                              = [bool]$modelOptions.GetProperty([Microsoft.SqlServer.Dac.Model.DatabaseOptions]::ArithAbortOn);
#         $options.AutoClose                                 = [bool]$modelOptions.GetProperty([Microsoft.SqlServer.Dac.Model.DatabaseOptions]::AutoClose);
#         $options.AutoCreateStatistics                      = [bool]$modelOptions.GetProperty([Microsoft.SqlServer.Dac.Model.DatabaseOptions]::AutoCreateStatistics);
#         $options.AutoShrink                                = [bool]$modelOptions.GetProperty([Microsoft.SqlServer.Dac.Model.DatabaseOptions]::AutoShrink);
#         $options.AutoUpdateStatistics                      = [bool]$modelOptions.GetProperty([Microsoft.SqlServer.Dac.Model.DatabaseOptions]::AutoUpdateStatistics);
#         $options.AutoUpdateStatisticsAsync                 = [bool]$modelOptions.GetProperty([Microsoft.SqlServer.Dac.Model.DatabaseOptions]::AutoUpdateStatisticsAsync);
#         $options.ChangeTrackingAutoCleanup                 = [bool]$modelOptions.GetProperty([Microsoft.SqlServer.Dac.Model.DatabaseOptions]::ChangeTrackingAutoCleanup);
#         $options.ChangeTrackingEnabled                     = [bool]$modelOptions.GetProperty([Microsoft.SqlServer.Dac.Model.DatabaseOptions]::ChangeTrackingEnabled);
#         $options.ChangeTrackingRetentionPeriod             = [int]$modelOptions.GetProperty([Microsoft.SqlServer.Dac.Model.DatabaseOptions]::ChangeTrackingRetentionPeriod);
#         $options.ChangeTrackingRetentionUnit               = [Microsoft.SqlServer.Dac.Model.TimeUnit]$modelOptions.GetProperty([Microsoft.SqlServer.Dac.Model.DatabaseOptions]::ChangeTrackingRetentionUnit);
#         $options.CompatibilityLevel                        = [int]$modelOptions.GetProperty([Microsoft.SqlServer.Dac.Model.DatabaseOptions]::CompatibilityLevel);
#         $options.ConcatNullYieldsNull                      = [bool]$modelOptions.GetProperty([Microsoft.SqlServer.Dac.Model.DatabaseOptions]::ConcatNullYieldsNull);
#         $options.Containment                               = [Microsoft.SqlServer.Dac.Model.Containment]$modelOptions.GetProperty([Microsoft.SqlServer.Dac.Model.DatabaseOptions]::Containment);
#         $options.CursorCloseOnCommit                       = [bool]$modelOptions.GetProperty([Microsoft.SqlServer.Dac.Model.DatabaseOptions]::CursorCloseOnCommit);
#         $options.CursorDefaultGlobalScope                  = [bool]$modelOptions.GetProperty([Microsoft.SqlServer.Dac.Model.DatabaseOptions]::CursorDefaultGlobalScope);
#         $options.DatabaseStateOffline                      = [bool]$modelOptions.GetProperty([Microsoft.SqlServer.Dac.Model.DatabaseOptions]::DatabaseStateOffline);
#         $options.DateCorrelationOptimizationOn             = [bool]$modelOptions.GetProperty([Microsoft.SqlServer.Dac.Model.DatabaseOptions]::DateCorrelationOptimizationOn);
#         $options.DefaultFullTextLanguage                   = [string]$modelOptions.GetProperty([Microsoft.SqlServer.Dac.Model.DatabaseOptions]::DefaultFullTextLanguage);
#         $options.DefaultLanguage                           = [string]$modelOptions.GetProperty([Microsoft.SqlServer.Dac.Model.DatabaseOptions]::DefaultLanguage);
#         $options.DBChainingOn                              = [bool]$modelOptions.GetProperty([Microsoft.SqlServer.Dac.Model.DatabaseOptions]::DBChainingOn);
#         $options.FileStreamDirectoryName                   = [string]$modelOptions.GetProperty([Microsoft.SqlServer.Dac.Model.DatabaseOptions]::FileStreamDirectoryName);
#         $options.FullTextEnabled                           = [bool]$modelOptions.GetProperty([Microsoft.SqlServer.Dac.Model.DatabaseOptions]::FullTextEnabled);
#         $options.HonorBrokerPriority                       = [bool]$modelOptions.GetProperty([Microsoft.SqlServer.Dac.Model.DatabaseOptions]::HonorBrokerPriority);
#         $options.NestedTriggersOn                          = [bool]$modelOptions.GetProperty([Microsoft.SqlServer.Dac.Model.DatabaseOptions]::NestedTriggersOn);
#         $options.NonTransactedFileStreamAccess             = [Microsoft.SqlServer.Dac.Model.NonTransactedFileStreamAccess]$modelOptions.GetProperty([Microsoft.SqlServer.Dac.Model.DatabaseOptions]::NonTransactedFileStreamAccess);
#         $options.NumericRoundAbortOn                       = [bool]$modelOptions.GetProperty([Microsoft.SqlServer.Dac.Model.DatabaseOptions]::NumericRoundAbortOn);
#         $options.PageVerifyMode                            = [Microsoft.SqlServer.Dac.Model.PageVerifyMode]$modelOptions.GetProperty([Microsoft.SqlServer.Dac.Model.DatabaseOptions]::PageVerifyMode);
#         $options.ParameterizationOption                    = [Microsoft.SqlServer.Dac.Model.ParameterizationOption]$modelOptions.GetProperty([Microsoft.SqlServer.Dac.Model.DatabaseOptions]::ParameterizationOption);
#         $options.QuotedIdentifierOn                        = [bool]$modelOptions.GetProperty([Microsoft.SqlServer.Dac.Model.DatabaseOptions]::QuotedIdentifierOn);
#         $options.ReadOnly                                  = [bool]$modelOptions.GetProperty([Microsoft.SqlServer.Dac.Model.DatabaseOptions]::ReadOnly);
#         $options.RecoveryMode                              = [Microsoft.SqlServer.Dac.Model.RecoveryMode]$modelOptions.GetProperty([Microsoft.SqlServer.Dac.Model.DatabaseOptions]::RecoveryMode);
#         $options.RecursiveTriggersOn                       = [bool]$modelOptions.GetProperty([Microsoft.SqlServer.Dac.Model.DatabaseOptions]::RecursiveTriggersOn);
#         $options.ServiceBrokerOption                       = [Microsoft.SqlServer.Dac.Model.ServiceBrokerOption]$modelOptions.GetProperty([Microsoft.SqlServer.Dac.Model.DatabaseOptions]::ServiceBrokerOption);
#         $options.SupplementalLoggingOn                     = [bool]$modelOptions.GetProperty([Microsoft.SqlServer.Dac.Model.DatabaseOptions]::SupplementalLoggingOn);
#         $options.TargetRecoveryTimePeriod                  = [int]$modelOptions.GetProperty([Microsoft.SqlServer.Dac.Model.DatabaseOptions]::TargetRecoveryTimePeriod);
#         $options.TargetRecoveryTimeUnit                    = [Microsoft.SqlServer.Dac.Model.TimeUnit]$modelOptions.GetProperty([Microsoft.SqlServer.Dac.Model.DatabaseOptions]::TargetRecoveryTimeUnit);
#         $options.TornPageProtectionOn                      = [bool]$modelOptions.GetProperty([Microsoft.SqlServer.Dac.Model.DatabaseOptions]::TornPageProtectionOn);
#         $options.TransformNoiseWords                       = [bool]$modelOptions.GetProperty([Microsoft.SqlServer.Dac.Model.DatabaseOptions]::TransformNoiseWords);
#         $options.Trustworthy                               = [bool]$modelOptions.GetProperty([Microsoft.SqlServer.Dac.Model.DatabaseOptions]::Trustworthy);
#         $options.TwoDigitYearCutoff                        = [int16]$modelOptions.GetProperty([Microsoft.SqlServer.Dac.Model.DatabaseOptions]::TwoDigitYearCutoff);
#         $options.VardecimalStorageFormatOn                 = [bool]$modelOptions.GetProperty([Microsoft.SqlServer.Dac.Model.DatabaseOptions]::VardecimalStorageFormatOn);
#         $options.UserAccessOption                          = [Microsoft.SqlServer.Dac.Model.UserAccessOption]$modelOptions.GetProperty([Microsoft.SqlServer.Dac.Model.DatabaseOptions]::UserAccessOption);
#         $options.WithEncryption                            = [bool]$modelOptions.GetProperty([Microsoft.SqlServer.Dac.Model.DatabaseOptions]::WithEncryption);

#         $options
#     }
# }