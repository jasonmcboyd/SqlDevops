function New-SchemaCompareEndpoint {
    [CmdletBinding()]
    param (
        [Parameter(
            Mandatory=$true,
            ParameterSetName='DacPac',
            Position=0,
            ValueFromPipelineByPropertyName=$true)]
        [string]
        $Path,

        [Parameter(
            Mandatory=$true,
            ParameterSetName='AuthenticateWithConnectionString',
            ValueFromPipelineByPropertyName=$true)]
        [string]
        $ConnectionString,

        [Parameter(
            Mandatory=$true,
            ParameterSetName='AuthenticateWithCredentials',
            ValueFromPipelineByPropertyName=$true)]
        [PSCredential]
        $Credential,

        [Parameter(
            Mandatory=$true,
            ParameterSetName='AuthenticateWithCredentials',
            ValueFromPipelineByPropertyName=$true)]
        [string]
        $Server,

        [Parameter(
            ParameterSetName='AuthenticateWithConnectionString',
            ValueFromPipelineByPropertyName=$true)]
        [Parameter(
            Mandatory=$true,
            ParameterSetName='AuthenticateWithCredentials',
            ValueFromPipelineByPropertyName=$true)]
        [string]
        $DatabaseName
    )

    process {
        if ($PSCmdlet.ParameterSetName -eq 'DacPac') {
            $Path = Normalize-Path $Path
            [Microsoft.SqlServer.Dac.Compare.SchemaCompareDacpacEndpoint]::new($Path)
        }
        else {
            $connectionEndpoint =
                New-DbConnectionEndpoint `
                    -ConnectionString $ConnectionString `
                    -Server $Server `
                    -Credential $Credential `
                    -DatabaseName $DatabaseName

            [Microsoft.SqlServer.Dac.Compare.SchemaCompareDatabaseEndpoint]::new($connectionEndpoint.ConnectionString)
        }
    }
}