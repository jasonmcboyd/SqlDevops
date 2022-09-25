function New-DbConnectionEndpoint {
    [CmdletBinding()]
    param (
        [Parameter(ValueFromPipelineByPropertyName=$true)]
        [string]
        $ConnectionString,

        [Parameter(ValueFromPipelineByPropertyName=$true)]
        [PSCredential]
        $Credential,

        [Parameter(ValueFromPipelineByPropertyName=$true)]
        [string]
        $Server,

        [Parameter(ValueFromPipelineByPropertyName=$true)]
        [string]
        $DatabaseName
    )

    $connectionStringBuilder = [System.Data.SqlClient.SqlConnectionStringBuilder]::new()

    $authenticateWithCredentials = $Credential -ne $null

    if ($authenticateWithCredentials) {
        if ([string]::IsNullOrWhiteSpace($Server)) {
            throw "The 'Server' parameter is mandatory when authenticating with credentials."
        }
        if ([string]::IsNullOrWhiteSpace($DatabaseName)) {
            throw "The 'DatabaseName' parameter is mandatory when authenticating with credentials."
        }

        $connectionStringBuilder.Add("Data Source", $Server)
        $connectionStringBuilder.Add("Initial Catalog", $DatabaseName)
        $connectionStringBuilder.Add("User Id", $Credential.UserName)
        $connectionStringBuilder.Add("Password", $Credential.GetNetworkCredential().Password)
    }
    else {
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
        elseif ([string]::IsNullOrWhiteSpace($connectionStringBuilder.InitialCatalog)) {
            throw "If authenticating with a connection string either the 'DatabaseName' parameter must be set or the connection string must specify a default database."
        }
    }

    [PSCustomObject]@{
        ConnectionString = $connectionStringBuilder.ConnectionString
        Server = $connectionStringBuilder.DataSource
        DatabaseName = $connectionStringBuilder.InitialCatalog
        UserId = $connectionStringBuilder.UserID
    }
}