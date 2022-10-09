param (
    [parameter(Mandatory=$true)]
    [ValidateSet('Major', 'Minor', 'Patch')]
    [string]
    $Segment,

    [parameter(Mandatory=$true)]
    [securestring]
    $ApiKey
)

& $PSScriptRoot\increment-version.ps1 $Segment
& $PSScriptRoot\build-module.ps1
& $PSScriptRoot\publish-module.ps1 $ApiKey