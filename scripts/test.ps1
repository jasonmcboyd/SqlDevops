pushd ~/wrk/dev/SqlDevOps
try {
  dotnet publish ./src/SqlDevOps
  ./Build/deploy-local.ps1
  Import-Module SqlDevOps
  New-SecureConnectionString -Server FCSFDIDBT2 -Database DealInsight_1 -IntegratedSecurity
}
finally {
  popd
}
