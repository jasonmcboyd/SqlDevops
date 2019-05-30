if ($IsCoreCLR) {
    Add-Type -Path (Join-Path $PSScriptRoot "../lib/Microsoft.SqlServer.DacFx.150.4240.1-preview/netcoreapp2.1/Microsoft.SqlServer.Dac.dll")
}
else {
    Add-Type -Path (Join-Path $PSScriptRoot "../lib/Microsoft.SqlServer.DacFx.150.4240.1-preview/net46/Microsoft.SqlServer.Dac.dll")
}