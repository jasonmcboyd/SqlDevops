function Export-DacPac {
    [CmdletBinding()]
    param (
        [Parameter(Mandatory=$true,ParameterSetName='NotConnectionString')]
        [string]
        $Server,

        [Parameter(Mandatory = $true,ParameterSetName='NotConnectionString')]
        [string]
        $DatabaseName,

        [Parameter(ParameterSetName='NotConnectionString')]
        [PSCredential]
        $Credential,

        [Parameter(Mandatory=$true,ParameterSetName='ConnectionString')]
        [string]
        $ConnectionString,

        [Parameter(Mandatory = $true)]
        [string]
        $Destination,

        [switch]
        $Overwrite
    )

    if ((Test-Path $Destination) -and !$Overwrite) {
        throw "File already exists."
    }

    $connectionStringBuilder = [System.Data.SqlClient.SqlConnectionStringBuilder]::new()

    if ($PSCmdlet.ParameterSetName -eq 'ConnectionString') {
        # HACK: For some reason setting the 'ConnectionString' property directly
        # results in an ArgumentException:
        #
        # Keyword not supported: 'ConnectionString'
        #
        # Found this workaround somewhere on the internet.
        $connectionStringBuilder.set_ConnectionString($ConnectionString)
        
        # Extract the database name from the connection string
        $DatabaseName = $connectionStringBuilder.InitialCatalog

        if ([string]::IsNullOrWhiteSpace($DatabaseName)) {
            throw "The database name must be included in the connection string."
        }
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

    $ErrorActionPreference = 'Stop'

    $Destination = $ExecutionContext.SessionState.Path.GetUnresolvedProviderPathFromPSPath($Destination)
    Write-Verbose "Exporting dacpac to $Destination."

    $service = [Microsoft.SqlServer.Dac.DacServices]::new($connectionStringBuilder.ConnectionString)
    $service.Extract($Destination, $DatabaseName, "SqlDevOps", [Version]::new(1,0,0,0))
    Get-Item -Path $Destination
}