Push-Location $PSScriptRoot
try {
    docker build --platform windows --rm -t sqldevops/build:latest .
    rm ./bin/* -force -recurse
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