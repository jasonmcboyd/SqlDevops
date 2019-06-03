Push-Location $PSScriptRoot
try {
    if (!(Test-Path ./bin)) {
        mkdir ./bin | out-null
    }
    else {
        rm ./bin/* -force -recurse
    }
    docker build --platform windows --rm -t sqldevops/build:latest .
    docker run `
        -t `
        --rm `
        --platform windows `
        -e VERSION=$Environment `
        -v "$(resolve-path ../src):c:/build/src:ro" `
        -v "$(resolve-path ./bin):c:/build/bin" `
        sqldevops/build
}
finally {
    Pop-Location
}