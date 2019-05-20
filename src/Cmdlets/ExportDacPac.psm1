function Export-DacPac {
    [CmdletBinding()]
    param (
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
        $ConnectionString,

        [Parameter(Mandatory = $true)]
        [string]
        $Destination,

        [switch]
        $Overwrite
    )

    if ((Test-Path $Destination) -and !$Overwrite) {
        Write-Error "File already exists."
    }

    if ([string]::IsNullOrWhiteSpace($Server)) {
        $Server = 'localhost'
    }

    $connectionStringBuilder = [System.Data.SqlClient.SqlConnectionStringBuilder]::new()

    if ($PSBoundParameters.ParameterSetName -eq 'ConnectionString') {
        $connectionStringBuilder.ConnectionString = $connectionString
    }
    else {
        $connectionStringBuilder.Add("Data Source", $Server)
        $connectionStringBuilder.Add("Initial Catalog", $DatabaseName)
        $connectionStringBuilder.Add("User Id", $Credential.UserName)
        $connectionStringBuilder.Add("Password", $Credential.GetNetworkCredential().Password)
    }

    $service = [Microsoft.SqlServer.Dac.DacServices]::new($connectionStringBuilder.ConnectionString)
    $service.Extract($Destination, $DatabaseName, "SqlDevOps", [Version]::new(1,0,0,0))
    Get-Item -Path $Destination
}