function Invoke-TSqlCodeAnalysis {
    [CmdletBinding()]
    param (
        [parameter(
            Mandatory=$true,
            ValueFromPipeline=$true,
            ValueFromPipelineByPropertyName=$true)]
        [Microsoft.SqlServer.Dac.Model.TSqlModel]
        $Model
    )

    process {
        $serviceFactory = [Microsoft.SqlServer.Dac.CodeAnalysis.CodeAnalysisServiceFactory]::new();
        $serviceFactory.CreateAnalysisService($Model);
    }
}