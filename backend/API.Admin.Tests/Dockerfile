# Usar a imagem do SDK do .NET 8.0 como base
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["backend/API.Admin.Tests/API.Admin.Tests.csproj", "backend/API.Admin.Tests/"]
COPY ["backend/API.Admin/API.Admin.csproj", "backend/API.Admin/"]
RUN dotnet restore "./backend/API.Admin.Tests/API.Admin.Tests.csproj"
COPY . .
WORKDIR "/src/backend/API.Admin.Tests"
RUN dotnet build "./API.Admin.Tests.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./API.Admin.Tests.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Usar a imagem do SDK do .NET 8.0 para executar os testes
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "test", "API.Admin.Tests.dll", "--logger:trx"]
