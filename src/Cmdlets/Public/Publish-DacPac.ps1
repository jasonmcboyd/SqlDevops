function Publish-DacPac {
    [CmdletBinding()]
    param (
        [Parameter(
            Mandatory=$true,
            Position=0,
            ValueFromPipeline=$true,
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
        $ErrorActionPreference = 'Stop'

        # Normalize and validate the path.
        $Path = Normalize-Path($Path)
        if (!(Test-Path $Path)) {
            throw "DacPac not found at: $Path"
        }

        $connectionEndpoint =
            New-DbConnectionEndpoint `
                -ConnectionString $ConnectionString `
                -Server $Server `
                -Credential $Credential `
                -DatabaseName $DatabaseName
        
        $service = [Microsoft.SqlServer.Dac.DacServices]::new($connectionEndpoint.ConnectionString)
        $package = [Microsoft.SqlServer.Dac.DacPackage]::Load($Path)
        $options = [Microsoft.SqlServer.Dac.PublishOptions]::new()
        $service.Publish($package, $connectionEndpoint.DatabaseName, $options)
    }
}