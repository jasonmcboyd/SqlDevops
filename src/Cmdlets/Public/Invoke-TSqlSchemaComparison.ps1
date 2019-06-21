function Invoke-TSqlSchemaComparison {
    [CmdletBinding()]
    param (
        [Parameter(
            Mandatory=$true,
            ValueFromPipelineByPropertyName=$true)]
        [Microsoft.SqlServer.Dac.Compare.SchemaCompareEndpoint]
        $Source,

        [Parameter(
            Mandatory=$true,
            ValueFromPipelineByPropertyName=$true)]
        [Microsoft.SqlServer.Dac.Compare.SchemaCompareEndpoint]
        $Target
    )

    process {
        [Microsoft.SqlServer.Dac.Compare.SchemaComparison]::new($Source, $Target).Compare()
    }
}