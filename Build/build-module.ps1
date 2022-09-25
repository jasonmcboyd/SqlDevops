[CmdletBinding()]
param (
)

Push-Location $PSScriptRoot
try {

    $csproj = [xml](Get-Content "$PsScripRoot/../src/SqlDevOps/SqlDevOps/SqlDevOps.csproj")

    $dacfxNugetVersion =
        $csproj.Project.ItemGroup.PackageReference `
        | Where-Object { $_ -match 'DacFx' } `
        | Select-Object -ExpandProperty Version

    if (!(Test-Path ./bin)) {
        mkdir ./bin | out-null
    }
    else {
        remove-item ./bin/* -force -recurse
    }

    docker build --rm --build-arg DACFX_VERSION=$dacfxNugetVersion -t sqldevops/build:latest -f Dockerfile .
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
