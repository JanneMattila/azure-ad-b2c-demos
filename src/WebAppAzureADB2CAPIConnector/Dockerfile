# This Dockerfile contains Build and Release steps:
# 1. Build image(https://hub.docker.com/_/microsoft-dotnet-core-sdk/)
FROM mcr.microsoft.com/dotnet/sdk:7.0.100-alpine3.16-amd64 AS build
WORKDIR /source

# Cache nuget restore
COPY /src/WebAppAzureADB2CAPIConnector/*.csproj .
RUN dotnet restore WebAppAzureADB2CAPIConnector.csproj

# Copy sources and compile
COPY /src/WebAppAzureADB2CAPIConnector .
RUN dotnet publish WebAppAzureADB2CAPIConnector.csproj --output /app/ --configuration Release

# 2. Release image
FROM mcr.microsoft.com/dotnet/aspnet:7.0.0-alpine3.16-amd64
WORKDIR /app
EXPOSE 80

# Copy content from Build image
COPY --from=build /app .

ENTRYPOINT ["dotnet", "WebAppAzureADB2CAPIConnector.dll"]
