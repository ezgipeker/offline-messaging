#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/OfflineMessaging.Api/OfflineMessaging.Api.csproj", "src/OfflineMessaging.Api/"]
RUN dotnet restore "src/OfflineMessaging.Api/OfflineMessaging.Api.csproj"
COPY . .
WORKDIR "/src/src/OfflineMessaging.Api"
RUN dotnet build "OfflineMessaging.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OfflineMessaging.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OfflineMessaging.Api.dll"]