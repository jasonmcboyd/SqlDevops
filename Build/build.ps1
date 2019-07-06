(get-item /usr/local/share/PackageManagement/NuGet/Packages/Microsoft.SqlServer.DacFx.*).fullname -match "DacFx\.(?<version>[^/]+)"
$version = $matches.version
write-host "version: $version"

copy-item `
    "/usr/local/share/PackageManagement/NuGet/Packages/Microsoft.SqlServer.DacFx.$($version)/lib/" `
    "/build/bin/SqlDevOps/lib/Microsoft.SqlServer.DacFx.$($version)" `
    -force `
    -recurse

get-childitem '/build/src' `
| where-object { $_.name -ne 'lib' } `
| copy-item -destination '/build/bin/SqlDevOps/' -force -recurse