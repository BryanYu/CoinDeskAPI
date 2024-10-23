# CoinDeskAPI
CodeDesk API Project 


## DB Startup

### Prerequisite
1. Install [Entity Framework Core Tool]("https://learn.microsoft.com/zh-tw/ef/core/cli/dotnet#installing-the-tools")
```
dotnet tool install --global dotnet-ef
```

### Migration Update
Add migration(if Migrations not exist)
```
cd .\CoidDesk
dotnet ef migrations add CurrencyTable --project .\CoinDesk.Infrastructure\CoinDesk.Infrastructure.csproj --startup-project .\CoinDesk.API\CoinDesk.API.csproj
```

DataBase Update(if Migrations exist)
```
cd .\CoidDesk
dotnet ef database update --project .\CoinDesk.Infrastructure\CoinDesk.Infrastructure.csproj --startup-project .\CoinDesk.API\CoinDesk.API.csproj
```
