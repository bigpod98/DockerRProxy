FROM mcr.microsoft.com/dotnet/sdk
RUN ./DockerRProxy /workdir
RUN dotnet publish -p:PublishSingleFile=true -p:PublishTrimmed=true -c Release -o /app --self-contained true -r lunx-x64
ENTRYPOINT [ "/app/DockerRProxy" ]
