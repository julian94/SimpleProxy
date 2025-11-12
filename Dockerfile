FROM mcr.microsoft.com/dotnet/sdk:10.0 AS dotnet-build
WORKDIR /build

COPY / .

RUN dotnet restore SimpleProxy.csproj
RUN dotnet build SimpleProxy.csproj --no-restore -c Release
RUN dotnet publish SimpleProxy.csproj --no-restore --no-build -c Release -o /publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime-env
WORKDIR /app

COPY --from=dotnet-build /publish .

ENTRYPOINT [ "dotnet", "SimpleProxy.dll" ]
