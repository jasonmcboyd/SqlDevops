param (
    [parameter(Mandatory=$true)]
    [ValidateSet('Major', 'Minor', 'Patch')]
    [string]
    $Segment
)

$manifestPath = "$PSScriptRoot\..\src\SqlDevOps.psd1"
$manifest = get-content "$manifestPath" -raw | invoke-expression
$version = [system.version]::new($manifest.ModuleVersion)

switch ($Segment) {
    "Major" { $newVersion = "$($version.Major + 1).0.0"}
    "Minor" { $newVersion = "$($version.Major).$($version.Minor + 1).0"}
    "Patch" { $newVersion = "$($version.Major).$($version.Minor).$($version.Build + 1)"}
}

((get-content "$manifestPath") -replace "ModuleVersion =.*", "ModuleVersion = '$newVersion'") | set-content "$manifestPath"