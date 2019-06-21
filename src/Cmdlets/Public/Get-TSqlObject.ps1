function Get-TSqlObject {
    [CmdletBinding()]
    param (
        [parameter(
            Mandatory=$true,
            ValueFromPipeline=$true,
            ValueFromPipelineByPropertyName=$true)]
        [Microsoft.SqlServer.Dac.Model.TSqlModel]
        $Model,

        [parameter(ValueFromPipelineByPropertyName=$true)]
        [string[]]
        $Name,

        [parameter(ValueFromPipelineByPropertyName=$true)]
        [ValidateSet(
            'Aggregate', 
            'ApplicationRole', 
            'Assembly', 
            'AssemblySource', 
            'AsymmetricKey', 
            'AuditAction', 
            'AuditActionGroup', 
            'AuditActionSpecification', 
            'BrokerPriority', 
            'BuiltInServerRole', 
            'Certificate', 
            'CheckConstraint', 
            'ClrTableOption', 
            'ClrTypeMethod', 
            'ClrTypeMethodParameter', 
            'ClrTypeProperty', 
            'Column', 
            'ColumnStoreIndex', 
            'Contract', 
            'Credential', 
            'CryptographicProvider', 
            'DatabaseAuditSpecification', 
            'DatabaseCredential', 
            'DatabaseDdlTrigger', 
            'DatabaseEncryptionKey', 
            'DatabaseEventNotification', 
            'DatabaseEventSession', 
            'DatabaseMirroringLanguageSpecifier', 
            'DatabaseOptions', 
            'DataCompressionOption', 
            'DataType', 
            'Default', 
            'DefaultConstraint', 
            'DmlTrigger', 
            'EdgeConstraint', 
            'Endpoint', 
            'ErrorMessage', 
            'EventGroup', 
            'EventSession', 
            'EventSessionAction', 
            'EventSessionDefinitions', 
            'EventSessionSetting', 
            'EventSessionTarget', 
            'EventTypeSpecifier', 
            'ExtendedProcedure', 
            'ExtendedProperty', 
            'ExternalDataSource', 
            'ExternalFileFormat', 
            'ExternalTable', 
            'Filegroup', 
            'FileTable', 
            'ForeignKeyConstraint', 
            'FullTextCatalog', 
            'FullTextIndex', 
            'FullTextIndexColumnSpecifier', 
            'FullTextStopList', 
            'HttpProtocolSpecifier', 
            'Index', 
            'LinkedServer', 
            'LinkedServerLogin', 
            'Login', 
            'MasterKey', 
            'MessageType', 
            'Parameter', 
            'PartitionFunction', 
            'PartitionScheme', 
            'PartitionValue', 
            'Permission', 
            'PrimaryKeyConstraint', 
            'Procedure', 
            'PromotedNodePathForSqlType', 
            'PromotedNodePathForXQueryType', 
            'Queue', 
            'QueueEventNotification', 
            'RemoteServiceBinding', 
            'ResourceGovernor', 
            'ResourcePool', 
            'Role', 
            'RoleMembership', 
            'Route', 
            'Rule', 
            'ScalarFunction', 
            'Schema', 
            'SearchProperty', 
            'SearchPropertyList', 
            'SecurityPolicy', 
            'SecurityPredicate', 
            'SelectiveXmlIndex', 
            'Sequence', 
            'ServerAudit', 
            'ServerAuditSpecification', 
            'ServerDdlTrigger', 
            'ServerEventNotification', 
            'ServerOptions', 
            'ServerRoleMembership', 
            'Service', 
            'ServiceBrokerLanguageSpecifier', 
            'Signature', 
            'SignatureEncryptionMechanism', 
            'SoapLanguageSpecifier', 
            'SoapMethodSpecification', 
            'SpatialIndex', 
            'SqlFile', 
            'Statistics', 
            'SymmetricKey', 
            'SymmetricKeyPassword', 
            'Synonym', 
            'Table', 
            'TableType', 
            'TableTypeCheckConstraint', 
            'TableTypeColumn', 
            'TableTypeDefaultConstraint', 
            'TableTypeIndex', 
            'TableTypePrimaryKeyConstraint', 
            'TableTypeUniqueConstraint', 
            'TableValuedFunction', 
            'TcpProtocolSpecifier', 
            'UniqueConstraint', 
            'User', 
            'UserDefinedServerRole', 
            'UserDefinedType', 
            'View', 
            'WorkloadGroup', 
            'XmlIndex', 
            'XmlNamespace', 
            'XmlSchemaCollection')]
        [string[]]
        $Type
    )

    process {
        $types = Get-TSqlTypeClassArray $Type
        foreach ($obj in $Model.GetObjects([Microsoft.SqlServer.Dac.Model.DacQueryScopes]::UserDefined, $types)) {
            [PSCustomObject]@{
                Model = $Model
                Object = $obj
            }
        }
    }
}