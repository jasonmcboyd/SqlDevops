FROM mcr.microsoft.com/powershell
WORKDIR /build
SHELL ["pwsh", "-Command"]
CMD ["pwsh", "./build.ps1"]

RUN iex (iwr 'https://chocolatey.org/install.ps1')
RUN choco install nuget.commandline -y
RUN nuget install Microsoft.SqlServer.DACFx -version 150.4240.1-preview -o packages

RUN set-content -path 'c:/build/build.ps1' -value "\"\
cp 'C:/build/packages/Microsoft.SqlServer.DacFx.150.4240.1-preview/lib' 'c:/build/bin/SqlDevOps/lib/Microsoft.SqlServer.DacFx.150.4240.1-preview' -force -recurse`r`n\
dir 'c:/build/src' | ?{`$_.name -ne 'Binaries'} | cp -destination 'c:/build/bin/SqlDevOps/' -force -recurse`r`n\""