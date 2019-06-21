# Load DacFX
if ($IsCoreCLR) {
    Add-Type -Path (Join-Path $PSScriptRoot "lib\Microsoft.SqlServer.DacFx.150.4240.1-preview\netcoreapp2.1\Microsoft.SqlServer.Dac.dll")
    Add-Type -Path (Join-Path $PSScriptRoot "lib\Microsoft.SqlServer.DacFx.150.4240.1-preview\netcoreapp2.1\Microsoft.SqlServer.Dac.Extensions.dll")
}
else {
    Add-Type -Path (Join-Path $PSScriptRoot "lib\Microsoft.SqlServer.DacFx.150.4240.1-preview\net46\Microsoft.SqlServer.Dac.dll")
    Add-Type -Path (Join-Path $PSScriptRoot "lib\Microsoft.SqlServer.DacFx.150.4240.1-preview\net46\Microsoft.SqlServer.Dac.Extensions.dll")
}

# Get public and private function definition files.
$Public  = Get-ChildItem -Path "$PSScriptRoot\Cmdlets\Public\*.ps1"
$Private = Get-ChildItem -Path "$PSScriptRoot\Cmdlets\Private\*.ps1"

# Dot source the files
foreach($import in @($Public + $Private)) {
    . $import.fullname
}