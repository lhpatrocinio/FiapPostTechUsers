# === RUNTIME BASE ===
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
USER root
RUN apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
RUN apk add --no-cache curl
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# === BUILD & RESTORE (SDK) ===
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copia apenas o csproj para restaurar primeiro
COPY ["src/Users.Api/Users.Api.csproj", "src/Users.Api/"]

RUN dotnet restore "src/Users.Api/Users.Api.csproj"

# Copia o resto do código
COPY . .

WORKDIR "/src/src/Users.Api"

RUN dotnet build "Users.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# === PUBLISH (SDK) ===
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Users.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# === FINAL RUNTIME IMAGE ===
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Users.Api.dll"]
