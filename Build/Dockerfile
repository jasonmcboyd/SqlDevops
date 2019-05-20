FROM mcr.microsoft.com/dotnet/core/sdk:2.1 AS dacfx
RUN dotnet new console -o /dacfx &&\
    dotnet add /dacfx/dacfx.csproj package Microsoft.SqlServer.DACFx --version 150.4240.1-preview --package-directory /dacfx/packages

# Build runtime image
FROM mcr.microsoft.com/powershell
COPY ./src/SqlDevOps.psd1 /usr/local/share/powershell/Modules/SqlDevOps/
COPY ./src/Cmdlets /usr/local/share/powershell/Modules/SqlDevOps/Cmdlets
COPY --from=dacfx /dacfx/packages/microsoft.sqlserver.dacfx/150.4240.1-preview/lib/netcoreapp2.1 /usr/local/share/powershell/Modules/SqlDevOps/Binaries
ENTRYPOINT ["pwsh"]