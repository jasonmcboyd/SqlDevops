function Remove-TSqlObject {
    [CmdletBinding()]
    param (
        [parameter(
            Mandatory=$true,
            ValueFromPipelineByPropertyName=$true)]
        [Microsoft.SqlServer.Dac.Model.TSqlModel]
        $Model,

        [parameter(
            Mandatory=$true,
            ValueFromPipeline=$true,
            ValueFromPipelineByPropertyName=$true)]
        [Microsoft.SqlServer.Dac.Model.TSqlObject[]]
        $Object
    )

    process {
        foreach ($obj in $Object)
        {
            $sourceInformation = $obj.GetSourceInformation()
            if ($null -eq $sourceInformation) {
                $sourceName = $obj.Name.ToString()
                $Model.ConvertToScriptedObject($obj, $sourceName)
            }
            else {
                $sourceName = $sourceInformation.SourceName
            }
            Write-Verbose "Deleting $($obj.Name.ToString())..."
            $Model.DeleteObjects($sourceName);
        }
    }
}