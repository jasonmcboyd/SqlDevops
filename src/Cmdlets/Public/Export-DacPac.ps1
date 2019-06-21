function Export-DacPac {
    [CmdletBinding()]
    param (
        [Parameter(
            Mandatory=$true,
            Position=0,
            ValueFromPipelineByPropertyName=$true)]
        [string]
        $Path,

        [Parameter(
            Mandatory=$true,
            ParameterSetName='Model',
            ValueFromPipeline=$true,
            ValueFromPipelineByPropertyName=$true)]
        [Microsoft.SqlServer.Dac.Model.TSqlModel]
        $Model,

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
        $DatabaseName,

        [Parameter(ParameterSetName='Model')]
        [switch]
        $IgnoreValidationErrors,

        [switch]
        $Overwrite
    )

    process {
        $ErrorActionPreference = 'Stop'

        $Path = Normalize-Path($Path)

        if (Test-Path $Path) {
            if ($Overwrite) {
                Write-Verbose "'$Path' exists but Overwrite switch was included."
            }
            else {
                throw "File already exists."
            }
        }

        if ($PSCmdlet.ParameterSetName -eq 'Model') {
            # Construct package options.
            # TODO: Figure out _exactly_ how IgnoreValidationErrors works.
            $packageOptions = [Microsoft.SqlServer.Dac.PackageOptions]::new()
            if ($IgnoreValidationErrors) {
                $packageOptions.IgnoreValidationErrors = [string[]]@("*")
            }
            
            # Construct package metadata.
            $packageMetadata = [Microsoft.SqlServer.Dac.PackageMetadata]::new()

            [Microsoft.SqlServer.Dac.DacPackageExtensions]::BuildPackage($Path, $Model, $packageMetadata, $packageOptions)
        }
        else {
            $connectionEndpoint =
                New-DbConnectionEndpoint `
                    -ConnectionString $ConnectionString `
                    -Server $Server `
                    -Credential $Credential `
                    -DatabaseName $DatabaseName

            Write-Verbose "Exporting dacpac to $Path."
            $service = [Microsoft.SqlServer.Dac.DacServices]::new($connectionEndpoint.ConnectionString)
            $service.Extract($Path, $connectionEndpoint.DatabaseName, "SqlDevOps", [Version]::new(1,0,0,0))
        }

        Get-Item -Path $Path
    }
}