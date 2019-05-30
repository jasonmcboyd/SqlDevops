function Publish-DacPac {
    [CmdletBinding()]
    param (
        [string]
        $Path,

        [Parameter(ParameterSetName='NotConnectionString')]
        [string]
        $Server,

        [Parameter(Mandatory = $true,ParameterSetName='NotConnectionString')]
        [string]
        $DatabaseName,

        [Parameter(ParameterSetName='NotConnectionString')]
        [PSCredential]
        $Credential,

        [Parameter(ParameterSetName='ConnectionString')]
        [string]
        $ConnectionString
    )

    if (!(Test-Path $Path)) {
        throw "DacPac not found at: $Path"
    }

    $connectionStringBuilder = [System.Data.SqlClient.SqlConnectionStringBuilder]::new()

    if ($PSCmdlet.ParameterSetName -eq 'ConnectionString') {
        $connectionStringBuilder.ConnectionString = $connectionString
    }
    else {
        if ([string]::IsNullOrWhiteSpace($Server)) {
            $Server = 'localhost'
        }
        
        $connectionStringBuilder.Add("Data Source", $Server)
        $connectionStringBuilder.Add("Initial Catalog", $DatabaseName)
        $connectionStringBuilder.Add("User Id", $Credential.UserName)
        $connectionStringBuilder.Add("Password", $Credential.GetNetworkCredential().Password)
    }

    $service = [Microsoft.SqlServer.Dac.DacServices]::new($connectionStringBuilder.ConnectionString)
    $package = [Microsoft.SqlServer.Dac.DacPackage]::Load($Path)
    $options = [Microsoft.SqlServer.Dac.PublishOptions]::new()
    $service.Publish($package, $DatabaseName, $options)
}