function Publish-DacPac {
    [CmdletBinding()]
    param (
        [Parameter(Mandatory=$true, Position=0)]
        [string]
        $Path,

        [Parameter(ParameterSetName='ConnectionString')]
        [string]
        $ConnectionString,

        [Parameter(Mandatory=$true, ParameterSetName='NotConnectionString')]
        [string]
        $Server,

        [Parameter(ParameterSetName='NotConnectionString')]
        [PSCredential]
        $Credential,

        [string]
        $DatabaseName
    )

    $ErrorActionPreference = 'Stop'

    if ($PSCmdlet.ParameterSetName -eq 'NotConnectionString' -and [string]::IsNullOrWhiteSpace($DatabaseName)) {
        throw "The DatabaseName parameter is mandatory when authenticating with credentials."
    }

    # Normalize and validate the path.
    $Path = Normalize-Path($Path)
    if (!(Test-Path $Path)) {
        throw "DacPac not found at: $Path"
    }

    $connectionStringBuilder = [System.Data.SqlClient.SqlConnectionStringBuilder]::new()

    if ($PSCmdlet.ParameterSetName -eq 'ConnectionString') {
        # HACK: For some reason setting the 'ConnectionString' property directly
        # results in an ArgumentException:
        #
        # Keyword not supported: 'ConnectionString'
        #
        # Found this workaround somewhere on the internet.
        $connectionStringBuilder.set_ConnectionString($connectionString)

        # If the DatabaseName parameter was passed set the Initial Catalog of the
        # connection string builder to the database.
        if (![string]::IsNullOrWhiteSpace($DatabaseName)) {
            $connectionStringBuilder.set_InitialCatalog($DatabaseName)
        }
    }
    else {
        $connectionStringBuilder.Add("Data Source", $Server)
        $connectionStringBuilder.Add("Initial Catalog", $DatabaseName)
        $connectionStringBuilder.Add("User Id", $Credential.UserName)
        $connectionStringBuilder.Add("Password", $Credential.GetNetworkCredential().Password)
    }
    
    $service = [Microsoft.SqlServer.Dac.DacServices]::new($connectionStringBuilder.ConnectionString)
    $package = [Microsoft.SqlServer.Dac.DacPackage]::Load($Path)
    $options = [Microsoft.SqlServer.Dac.PublishOptions]::new()
    $service.Publish($package, $connectionStringBuilder.InitialCatalog, $options)
}