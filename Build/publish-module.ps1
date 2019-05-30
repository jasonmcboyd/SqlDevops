param (
    [parameter(Mandatory=$true)]
    [string]
    $ApiKey

)
Publish-Module `
    -Path (Join-Path $PSScriptRoot "/bin/SqlDevOps" | Resolve-Path) `
    -NuGetApiKey $ApiKey