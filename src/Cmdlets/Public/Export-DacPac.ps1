function Export-DacPac {
    [CmdletBinding()]
    param (
        [Parameter(Mandatory=$true, Position=0, ValueFromPipeline=$true, ValueFromPipelineByPropertyName=$true)]
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
        $DatabaseName,

        [switch]
        $Overwrite
    )

    process {
        $ErrorActionPreference = 'Stop'

        $Path = Normalize-Path($Path)

        if ((Test-Path $Path) -and !$Overwrite) {
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

        Write-Verbose "Exporting dacpac to $Path."
        $service = [Microsoft.SqlServer.Dac.DacServices]::new($connectionStringBuilder.ConnectionString)
        $service.Extract($Path, $connectionStringBuilder.InitialCatalog, "SqlDevOps", [Version]::new(1,0,0,0))
        Get-Item -Path $Path
    }
}