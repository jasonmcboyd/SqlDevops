function Get-TSqlTypeClassArray {
    [CmdletBinding()]
    param (
        [string[]]
        $Type
    )

    [Microsoft.SqlServer.Dac.Model.ModelTypeClass[]](
        $Type `
        | Foreach-Object {
            switch ($_) {
                'Aggregate' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::Aggregate }
                'ApplicationRole' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::ApplicationRole }
                'Assembly' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::Assembly }
                'AssemblySource' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::AssemblySource }
                'AsymmetricKey' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::AsymmetricKey }
                'AuditAction' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::AuditAction }
                'AuditActionGroup' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::AuditActionGroup }
                'AuditActionSpecification' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::AuditActionSpecification }
                'BrokerPriority' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::BrokerPriority }
                'BuiltInServerRole' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::BuiltInServerRole }
                'Certificate' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::Certificate }
                'CheckConstraint' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::CheckConstraint }
                'ClrTableOption' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::ClrTableOption }
                'ClrTypeMethod' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::ClrTypeMethod }
                'ClrTypeMethodParameter' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::ClrTypeMethodParameter }
                'ClrTypeProperty' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::ClrTypeProperty }
                'Column' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::Column }
                'ColumnStoreIndex' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::ColumnStoreIndex }
                'Contract' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::Contract }
                'Credential' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::Credential }
                'CryptographicProvider' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::CryptographicProvider }
                'DatabaseAuditSpecification' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::DatabaseAuditSpecification }
                'DatabaseCredential' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::DatabaseCredential }
                'DatabaseDdlTrigger' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::DatabaseDdlTrigger }
                'DatabaseEncryptionKey' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::DatabaseEncryptionKey }
                'DatabaseEventNotification' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::DatabaseEventNotification }
                'DatabaseEventSession' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::DatabaseEventSession }
                'DatabaseMirroringLanguageSpecifier' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::DatabaseMirroringLanguageSpecifier }
                'DatabaseOptions' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::DatabaseOptions }
                'DataCompressionOption' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::DataCompressionOption }
                'DataType' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::DataType }
                'Default' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::Default }
                'DefaultConstraint' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::DefaultConstraint }
                'DmlTrigger' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::DmlTrigger }
                'EdgeConstraint' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::EdgeConstraint }
                'Endpoint' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::Endpoint }
                'ErrorMessage' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::ErrorMessage }
                'EventGroup' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::EventGroup }
                'EventSession' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::EventSession }
                'EventSessionAction' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::EventSessionAction }
                'EventSessionDefinitions' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::EventSessionDefinitions }
                'EventSessionSetting' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::EventSessionSetting }
                'EventSessionTarget' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::EventSessionTarget }
                'EventTypeSpecifier' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::EventTypeSpecifier }
                'ExtendedProcedure' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::ExtendedProcedure }
                'ExtendedProperty' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::ExtendedProperty }
                'ExternalDataSource' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::ExternalDataSource }
                'ExternalFileFormat' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::ExternalFileFormat }
                'ExternalTable' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::ExternalTable }
                'Filegroup' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::Filegroup }
                'FileTable' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::FileTable }
                'ForeignKeyConstraint' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::ForeignKeyConstraint }
                'FullTextCatalog' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::FullTextCatalog }
                'FullTextIndex' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::FullTextIndex }
                'FullTextIndexColumnSpecifier' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::FullTextIndexColumnSpecifier }
                'FullTextStopList' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::FullTextStopList }
                'HttpProtocolSpecifier' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::HttpProtocolSpecifier }
                'Index' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::Index }
                'LinkedServer' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::LinkedServer }
                'LinkedServerLogin' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::LinkedServerLogin }
                'Login' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::Login }
                'MasterKey' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::MasterKey }
                'MessageType' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::MessageType }
                'Parameter' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::Parameter }
                'PartitionFunction' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::PartitionFunction }
                'PartitionScheme' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::PartitionScheme }
                'PartitionValue' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::PartitionValue }
                'Permission' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::Permission }
                'PrimaryKeyConstraint' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::PrimaryKeyConstraint }
                'Procedure' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::Procedure }
                'PromotedNodePathForSqlType' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::PromotedNodePathForSqlType }
                'PromotedNodePathForXQueryType' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::PromotedNodePathForXQueryType }
                'Queue' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::Queue }
                'QueueEventNotification' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::QueueEventNotification }
                'RemoteServiceBinding' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::RemoteServiceBinding }
                'ResourceGovernor' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::ResourceGovernor }
                'ResourcePool' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::ResourcePool }
                'Role' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::Role }
                'RoleMembership' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::RoleMembership }
                'Route' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::Route }
                'Rule' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::Rule }
                'ScalarFunction' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::ScalarFunction }
                'Schema' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::Schema }
                'SearchProperty' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::SearchProperty }
                'SearchPropertyList' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::SearchPropertyList }
                'SecurityPolicy' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::SecurityPolicy }
                'SecurityPredicate' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::SecurityPredicate }
                'SelectiveXmlIndex' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::SelectiveXmlIndex }
                'Sequence' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::Sequence }
                'ServerAudit' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::ServerAudit }
                'ServerAuditSpecification' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::ServerAuditSpecification }
                'ServerDdlTrigger' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::ServerDdlTrigger }
                'ServerEventNotification' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::ServerEventNotification }
                'ServerOptions' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::ServerOptions }
                'ServerRoleMembership' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::ServerRoleMembership }
                'Service' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::Service }
                'ServiceBrokerLanguageSpecifier' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::ServiceBrokerLanguageSpecifier }
                'Signature' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::Signature }
                'SignatureEncryptionMechanism' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::SignatureEncryptionMechanism }
                'SoapLanguageSpecifier' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::SoapLanguageSpecifier }
                'SoapMethodSpecification' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::SoapMethodSpecification }
                'SpatialIndex' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::SpatialIndex }
                'SqlFile' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::SqlFile }
                'Statistics' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::Statistics }
                'SymmetricKey' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::SymmetricKey }
                'SymmetricKeyPassword' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::SymmetricKeyPassword }
                'Synonym' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::Synonym }
                'Table' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::Table }
                'TableType' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::TableType }
                'TableTypeCheckConstraint' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::TableTypeCheckConstraint }
                'TableTypeColumn' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::TableTypeColumn }
                'TableTypeDefaultConstraint' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::TableTypeDefaultConstraint }
                'TableTypeIndex' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::TableTypeIndex }
                'TableTypePrimaryKeyConstraint' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::TableTypePrimaryKeyConstraint }
                'TableTypeUniqueConstraint' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::TableTypeUniqueConstraint }
                'TableValuedFunction' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::TableValuedFunction }
                'TcpProtocolSpecifier' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::TcpProtocolSpecifier }
                'UniqueConstraint' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::UniqueConstraint }
                'User' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::User }
                'UserDefinedServerRole' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::UserDefinedServerRole }
                'UserDefinedType' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::UserDefinedType }
                'View' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::View }
                'WorkloadGroup' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::WorkloadGroup }
                'XmlIndex' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::XmlIndex }
                'XmlNamespace' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::XmlNamespace }
                'XmlSchemaCollection' { [Microsoft.SqlServer.Dac.Model.ModelSchema]::XmlSchemaCollection }
            }
        })
}