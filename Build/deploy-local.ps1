[CmdletBinding()]
param ()

Set-StrictMode -Version 'Latest'
$ErrorActionPreference = 'Stop'

$destinationPath = Join-Path (Split-Path $profile.CurrentUserAllHosts) 'Modules/SqlDevOps'
$sourcePath = "$PSScriptRoot/../src/SqlDevOps/SqlDevOps/bin/Debug/net5.0/publish"

if (!(Test-Path $destinationPath)) {
  mkdir $destinationPath
}

Remove-Item "$destinationPath/*" -Force -Recurse

Copy-Item "$sourcePath/*" $destinationPath
