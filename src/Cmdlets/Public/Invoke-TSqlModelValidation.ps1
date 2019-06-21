function Invoke-TSqlModelValidation {
    [CmdletBinding()]
    param(
        [parameter(
            Mandatory=$true,
            ValueFromPipeline=$true,
            ValueFromPipelineByPropertyName=$true)]
        [Microsoft.SqlServer.Dac.Model.TSqlModel]
        $Model
    )

    process {
        $Model.Validate()
    }
}