# CoinDeskAPI
CodeDesk API Project 


## DB Startup
1. Install [Entity Framework Core Tool]("https://learn.microsoft.com/zh-tw/ef/core/cli/dotnet#installing-the-tools")
```
dotnet tool install --global dotnet-ef
```
2. Use Migrations To Update DataBase
```
cd .\CoinDeskAPI
dotnet ef database update --project .\CoinDesk.Infrastructure\CoinDesk.Infrastructure.csproj --startup-project .\CoinDesk.API\CoinDesk.API.csproj
```

