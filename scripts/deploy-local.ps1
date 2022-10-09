[CmdletBinding()]
param ()

Set-StrictMode -Version 'Latest'
$ErrorActionPreference = 'Stop'

$destinationPath = Join-Path (Split-Path $profile.CurrentUserAllHosts) 'Modules/SqlDevOps'
$sourcePath = "$PSScriptRoot/../src/SqlDevOps/bin/Debug/net6.0/win-x64/publish"

if (!(Test-Path $destinationPath)) {
  mkdir $destinationPath
}

Remove-Item "$destinationPath/*" -Force -Recurse

Copy-Item "$sourcePath/*" $destinationPath
