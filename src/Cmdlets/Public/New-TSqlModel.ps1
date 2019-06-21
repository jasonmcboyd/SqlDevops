function New-TSqlModel {
    [CmdletBinding()]
    param (
        [Parameter(
            Mandatory=$true,
            ParameterSetName='FromDisk',
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
        if ($PSCmdlet.ParameterSetName -eq 'FromDisk') {
            
            # Normalize and validate the path.
            $Path = Normalize-Path($Path)
            if (!(Test-Path $Path)) {
                throw "Path not found at: $Path"
            }

            if ((Get-Item $Path).PSIsContainer) {
                $model = [Microsoft.SqlServer.Dac.Model.TSqlModel]::new([Microsoft.SqlServer.Dac.Model.SqlServerVersion]::Sql150, [Microsoft.SqlServer.Dac.Model.TSqlModelOptions]::new())

                foreach ($file in (Get-ChildItem -Path $Path -Recurse -Filter '*.sql')) {
                    $model.AddObjects((Get-Content -Path $file.Fullname -Raw))
                }
            }
            else {
                try {
                    $dacpacStream = [System.IO.File]::Open($Path, [System.IO.FileMode]::Open)
                    $modelLoadOptions = [Microsoft.SqlServer.Dac.Model.ModelLoadOptions]::new([Microsoft.SqlServer.Dac.DacSchemaModelStorageType]::Memory, $true)
                    $model = [Microsoft.SqlServer.Dac.Model.TSqlModel]::LoadFromDacpac($dacpacStream, $modelLoadOptions)
                }
                finally {
                    $dacpacStream.Dispose()
                }
            }
        }
        else {

            $connectionEndpoint =
                New-DbConnectionEndpoint `
                    -ConnectionString $ConnectionString `
                    -Server $Server `
                    -Credential $Credential `
                    -DatabaseName $DatabaseName
            $modelOptions = [Microsoft.SqlServer.Dac.Model.ModelExtractOptions]::new()
            $modelOptions.LoadAsScriptBackedModel = $true
            $modelOptions.Storage = [Microsoft.SqlServer.Dac.DacSchemaModelStorageType]::Memory
            $model = [Microsoft.SqlServer.Dac.Model.TSqlModel]::LoadFromDatabase($connectionEndpoint.ConnectionString, $modelLoadOptions)
        }

        $model
        # [PSCustomObject]@{
        #     PSTypeName = 'SqlDevops.TSqlModel'
        #     TSqlModel = $model
        # }
    }
}