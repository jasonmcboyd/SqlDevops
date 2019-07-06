FROM mcr.microsoft.com/powershell
WORKDIR /build
SHELL ["pwsh", "-Command"]
CMD ["pwsh", "./build.ps1"]
ADD build.ps1 .
ARG DACFX_VERSION

RUN register-packagesource -name NuGet -providername Nuget -Location https://www.nuget.org/api/v2
RUN install-package Microsoft.SqlServer.DACFx -providername nuget -requiredversion $env:DACFX_VERSION -force