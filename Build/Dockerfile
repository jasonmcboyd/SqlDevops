FROM mcr.microsoft.com/powershell
WORKDIR /build
SHELL ["pwsh", "-Command"]
CMD ["pwsh", "./build.ps1"]

RUN iex (iwr 'https://chocolatey.org/install.ps1')
RUN choco install nuget.commandline -y
RUN nuget install Microsoft.SqlServer.DACFx -version 150.4240.1-preview -o packages

COPY ./SqlDevOps.nuspec c:/build/SqlDevOps.nuspec

RUN set-content -path 'c:/build/build.ps1' -value "\"\
mkdir ./bin/Binaries`r`n\
cp 'C:/build/packages/Microsoft.SqlServer.DacFx.150.4240.1-preview/lib/netcoreapp2.1/*.dll' 'c:/build/bin/Binaries/'`r`n\
dir 'c:/build/src' | ?{`$_.name -ne 'Binaries'} | cp -destination 'c:/build/bin/' -force`r`n\
cp ./SqlDevOps.nuspec ./bin/SqlDevOps.nuspec`r`n\
(gc ./bin/SqlDevOps.nuspec -raw) -replace '<version></version>', `\"<version>`$env:version</version>`\" | set-content ./bin/SqlDevOps.nuspec`r`n\
cd ./bin`r`n\
nuget pack \"";
