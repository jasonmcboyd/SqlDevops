function Export-TSqlSchema {
    [CmdletBinding()]
    param (
        [parameter(
            Mandatory=$true,
            Position=0,
            ValueFromPipelineByPropertyName=$true)]
        [string]
        $Path,

        [parameter(
            Mandatory=$true,
            Position=1,
            ValueFromPipeline=$true,
            ValueFromPipelineByPropertyName=$true)]
        [Microsoft.SqlServer.Dac.Model.TSqlModel]
        $Model,

        [ValidateSet("type/schema", "schema/type")]
        [string]
        $DirectoryStructure = "type/schema",

        [switch]
        $Overwrite
    )

    process {

        $Path = Normalize-Path($Path)

        $objects =
            $Model.GetObjects([Microsoft.SqlServer.Dac.Model.DacQueryScopes]::UserDefined) `
            | Where-Object { $_.Name.HasName }

        foreach ($obj in $objects) {
            
            $schema = $obj.Name.Parts[0].Replace('\', '_')
            $filename = "$($obj.Name.Parts[$obj.Name.Parts.Count - 1].Replace('\', '_')).sql"
            
            if ($DirectoryStructure -eq "type/schema") {
                $folder = Join-Path (Join-Path $Path $obj.ObjectType.Name) $schema
            }
            else {
                $folder = Join-Path (Join-Path $Path $schema) $obj.ObjectType.Name 
            }

            for ($i = 1; $i -lt $obj.Name.Parts.Count - 2; $i++) {
                $folder = Join-Path $folder $obj.Name.Parts[$i].Replace('\', '_')
            }

            if (!(Test-Path $folder)) {
                New-Item -Path $folder -ItemType Directory
            }
            $file = Join-Path $folder $filename
            $obj.GetScript() | Set-Content -Path $file
        }
    }
}