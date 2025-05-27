# Etapa base para runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Etapa para build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia e restaura os projetos
COPY ["MonolitoBackend.Api/MonolitoBackend.Api.csproj", "MonolitoBackend.Api/"]
COPY ["MonolitoBackend.Core/MonolitoBackend.Core.csproj", "MonolitoBackend.Core/"]
COPY ["MonolitoBackend.Infrastructure/MonolitoBackend.Infrastructure.csproj", "MonolitoBackend.Infrastructure/"]
RUN dotnet restore "MonolitoBackend.Api/MonolitoBackend.Api.csproj"

# Copia todos os arquivos e compila
COPY . .
WORKDIR "/src/MonolitoBackend.Api"
RUN dotnet build -c Release -o /app/build

# Publica o projeto
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Etapa final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MonolitoBackend.Api.dll"]

