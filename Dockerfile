FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source
COPY ["src/Application/Application.csproj", "./src/Application/"]
COPY ["src/Domain/Domain.csproj", "./src/Domain/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "./src/Infrastructure/"]
COPY ["src/WebApi/WebApi.csproj", "./src/WebApi/"]
COPY ["tests/Application.UnitTests/Application.UnitTests.csproj", "./tests/Application.UnitTests/"]
COPY ["tests/Application.IntegrationTests/Application.IntegrationTests.csproj", "./tests/Application.IntegrationTests/"]
COPY ["tests/Domain.UnitTests/Domain.UnitTests.csproj", "./tests/Domain.UnitTests/"]
COPY ["tests/Domain.IntegrationTests/Domain.IntegrationTests.csproj", "./tests/Domain.IntegrationTests/"]
COPY ["tests/Infrastructure.UnitTests/Infrastructure.UnitTests.csproj", "./tests/Infrastructure.UnitTests/"]
COPY ["tests/Infrastructure.IntegrationTests/Infrastructure.IntegrationTests.csproj", "./tests/Infrastructure.IntegrationTests/"]
COPY ["tests/WebApi.IntegrationTests/WebApi.IntegrationTests.csproj", "./tests/WebApi.IntegrationTests/"]
COPY ["Services.StateMachines.sln", "./"]

RUN dotnet restore "./Services.StateMachines.sln"

COPY . .

FROM build AS publish
WORKDIR /source
RUN dotnet publish "./src/WebApi/WebApi.csproj" -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 80
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicroDotNet.Services.StateMachines.WebApi.dll"]
