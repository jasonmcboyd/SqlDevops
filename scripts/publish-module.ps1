param (
    [parameter(Mandatory=$true)]
    [securestring]
    $ApiKey
)

Publish-Module `
    -Path (Join-Path $PSScriptRoot "/bin/SqlDevOps" | Resolve-Path) `
    -NuGetApiKey ([System.Runtime.InteropServices.marshal]::PtrToStringAuto([System.Runtime.InteropServices.marshal]::SecureStringToBSTR($ApiKey)))