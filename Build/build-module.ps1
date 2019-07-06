[CmdletBinding()]
param (
    [string]
    $DacFxVersion = "150.4451.1-preview"
)

Push-Location $PSScriptRoot
try {
    if (!(Test-Path ./bin)) {
        mkdir ./bin | out-null
    }
    else {
        remove-item ./bin/* -force -recurse
    }
    
    docker build --rm --build-arg DACFX_VERSION=$DacFxVersion -t sqldevops/build:latest -f Linux.Dockerfile .
    docker run `
        -t `
        --rm `
        -v "$(resolve-path ../src):/build/src:ro" `
        -v "$(resolve-path ./bin):/build/bin" `
        sqldevops/build
}
finally {
    Pop-Location
}