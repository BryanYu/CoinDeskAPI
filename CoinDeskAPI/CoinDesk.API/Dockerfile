FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CoinDesk.API/CoinDesk.API.csproj", "CoinDesk.API/"]
COPY ["CodeDesk.Service/CodeDesk.Service.csproj", "CodeDesk.Service/"]
COPY ["CoinDesk.Model/CoinDesk.Model.csproj", "CoinDesk.Model/"]
COPY ["CoinDesk.Domain/CoinDesk.Domain.csproj", "CoinDesk.Domain/"]
COPY ["CoinDesk.Infrastructure/CoinDesk.Infrastructure.csproj", "CoinDesk.Infrastructure/"]
COPY ["CoinDesk.Utility/CoinDesk.Utility.csproj", "CoinDesk.Utility/"]
RUN dotnet restore "CoinDesk.API/CoinDesk.API.csproj"
COPY . .
WORKDIR "/src/CoinDesk.API"
RUN dotnet build "CoinDesk.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "CoinDesk.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CoinDesk.API.dll"]
