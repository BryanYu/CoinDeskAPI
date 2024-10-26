# CoinDeskAPI

### 建立資料庫與資料表的語法
```
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'CurrencyDB')
BEGIN
    CREATE DATABASE [CurrencyDB];
END
GO

USE [CurrencyDB];
GO

IF OBJECT_ID(N'[Currency]') IS NULL
BEGIN
    CREATE TABLE [Currency] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(20) NOT NULL,
        [CurrencyCode] nvarchar(max) NOT NULL,
        [CreatedTime] datetime2 NOT NULL,
        [UpdateTime] datetime2 NULL,
        CONSTRAINT [PK_Currency] PRIMARY KEY ([Id])
    );
    DECLARE @defaultSchema AS sysname;
    SET @defaultSchema = SCHEMA_NAME();
    DECLARE @description AS sql_variant;
    SET @description = N'幣別Id';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', @defaultSchema, 'TABLE', N'Currency', 'COLUMN', N'Id';
    SET @description = N'幣別名稱';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', @defaultSchema, 'TABLE', N'Currency', 'COLUMN', N'Name';
    SET @description = N'幣別代碼';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', @defaultSchema, 'TABLE', N'Currency', 'COLUMN', N'CurrencyCode';        
    SET @description = N'建立時間';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', @defaultSchema, 'TABLE', N'Currency', 'COLUMN', N'CreatedTime';
    SET @description = N'修改時間';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', @defaultSchema, 'TABLE', N'Currency', 'COLUMN', N'UpdateTime';
END;
GO
```



### Docker Support

由於mssqllocaldb不支援遠端連線，所以佈署在Docker的api專案無法連線至本機的mssqllocaldb，我改用docker compose建立一個sql server的container，讓api專案連到這個container中，並在應用程式啟動時自動執行migrations建立資料庫

docker-compose.yaml內容如下
```
name: coindeskapi-docker

services:
  coindesk-api:
    image: coindeskapi:latest
    container_name: coindesk-api
    build:
      context: .
      dockerfile: CoinDesk.API/Dockerfile
    ports:
      - 8080:8080
    environment:
      - ASPNETCORE_ENVIRONMENT=Docker
    depends_on:
      - sql-server-docker

  sql-server-docker:
    hostname: sql-server-docker
    container_name: sql-server-docker
    image: mcr.microsoft.com/mssql/server:2022-latest
    ports:
      - 1433:1433
    environment:
      - ACCEPT_EULA=Y
      - SA_PASSWORD=Aa123456
```

執行指令
```
cd .\CoinDeskAPI\
docker compose up -d --build
```

啟動瀏覽器並連線至[swagger](http://localhost:8080/swagger/index.html)，即可查看swagger文件