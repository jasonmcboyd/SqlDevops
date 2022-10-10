[CmdletBinding()]
param ()

Set-StrictMode -Version 'Latest'
$ErrorActionPreference = 'Stop'

Push-Location "$PSScriptRoot/../src/SqlDevOps"
try {
    dotnet clean .

    Get-ChildItem -Recurse -Directory `
    | Where-Object { $_.Name -match '^(bin|obj)$' } `
    | Remove-Item -Recurse -Force

    dotnet build .
    dotnet publish . --os win
}
finally {
  Pop-Location
}
