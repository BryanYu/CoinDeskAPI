# CoinDeskAPI
此專案包含
1. API 站台 (Currency CRUD)
2. 單元測試案例
  

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


## 實作加分題
- [x] 印出所有 API 被呼叫以及呼叫外部 API 的 request and response body log
- [x] Error handling 處理 API response
- [x] swagger-ui
- [x] 多語系設計
- [x] design pattern 實作
- [x] 能夠運行在 Docker
- [x] 加解密技術應用 (AES/RSA…etc.)


## 印出所有 API 被呼叫以及呼叫外部 API 的 request and response body log
1. 我使用Serilog輸出至Console與File中，並開發自訂的Middleware紀錄，可參考[HttpLoggingMiddleware.cs](https://github.com/BryanYu/CoinDeskAPI/blob/master/CoinDeskAPI/CoinDesk.API/Middleware/HttpLoggingMiddleware.cs)
2. 外部呼叫API的Log，我實作DelegatingHandler並在建立HttpClient時帶入，可參考[LoggingHttpMessageHandler.cs](https://github.com/BryanYu/CoinDeskAPI/blob/master/CoinDeskAPI/CoinDesk.API/Handler/LoggingHttpMessageHandler.cs)
3. Serilog輸出的設定檔可參考[appsettings.json](https://github.com/BryanYu/CoinDeskAPI/blob/master/CoinDeskAPI/CoinDesk.API/appsettings.json)內的`Serilog`的相關設定


## Error handling 處理 API response
1. 當發生Exception時，將Exception拋送至外部讓統一的Middleware處理。我使用內建的`IExceptionHandler`介面實作[GlobalExceptionHandler](https://github.com/BryanYu/CoinDeskAPI/blob/master/CoinDeskAPI/CoinDesk.API/Handler/GlobalExceptionHandler.cs)，並在Middleware注入時使用`app.UseExceptionHandler()`，讓整個API應用程式都可以使用
2. API應用程式統一回傳格式
   1. 當模型驗證發生錯誤時，需要統一的回傳格式並回應錯誤訊息，我使用內建的`IActionFilter`，並實作[ModelValidateActionFilter](https://github.com/BryanYu/CoinDeskAPI/blob/master/CoinDeskAPI/CoinDesk.API/ActionFilter/ModelValidateActionFilter.cs)，讓模型發生驗證錯誤時，可以處理錯誤訊息並回傳
   2. 當API回應成功或其他狀態時，我使用內建的`IResultFilter`，並實作[GlobalResponseActionFilter](https://github.com/BryanYu/CoinDeskAPI/blob/master/CoinDeskAPI/CoinDesk.API/ActionFilter/GlobalResponseActionFilter.cs)去建立統一的回傳格式與訊息

## swagger-ui
設定位置 [SwaagerGenExtension.cs](https://github.com/BryanYu/CoinDeskAPI/blob/master/CoinDeskAPI/CoinDesk.API/Extension/SwaagerGenExtension.cs)
1. 使用內建的swagger文件產生API可測試站台
2. 專案設定產生xml，並套用至swagger框架上，在產生時即可將xml註解放至swagger畫面上([可參考設定位置](https://github.com/BryanYu/CoinDeskAPI/blob/master/CoinDeskAPI/CoinDesk.API/Extension/SwaagerGenExtension.cs#L14))
3. 因加入多語系功能，需要在Header帶入多語系設定。使用swagger框架中的`IOperationFilter`介面，開發[AcceptLanguageHeaderOperationFilter](https://github.com/BryanYu/CoinDeskAPI/blob/65390671e02946fc73a4cbbab499695ade15309a/CoinDeskAPI/CoinDesk.API/OptionFilter/AcceptLanguageOptionFilter.cs)，並加入到swagger框架設定中([可參考設定位置](https://github.com/BryanYu/CoinDeskAPI/blob/65390671e02946fc73a4cbbab499695ade15309a/CoinDeskAPI/CoinDesk.API/OptionFilter/AcceptLanguageOptionFilter.cs#L6))
4. 因加入Bearer Token Auth 功能，需要在Header帶入Authorization設定。我使用swagger框架內建的`AddSecurityDefinition`設定([可參考設定位置](https://github.com/BryanYu/CoinDeskAPI/blob/master/CoinDeskAPI/CoinDesk.API/Extension/SwaagerGenExtension.cs#L19))


## 多語系設計
1. 當Client呼叫時，帶入Header(Accept-Language)，即可使用多語系功能。並在API應用程式加入Middleware`app.UseRequestLocalization`啟用多語系
2. 使用Resx建立多語系字詞，並透過自訂的[LocalizeService](https://github.com/BryanYu/CoinDeskAPI/blob/master/CoinDeskAPI/CodeDesk.Service/Implements/LocalizeService.cs)與內建的`IStringLocalizer`搭配，使用的地方有
   1. `GetCurrency`回傳時的資料內
   2. `GlobalResponseActionFilter`
   3. `ModelValidateActionFilter`
3. Resx透過prefix搭配列舉做一些功能分類，未來也可視情況拆分成不同Resx檔案進行管理
   
## design pattern 實作
1. API專案整體使用Mediator Pattern與Command Pattern，並使用[MediateR]套件(https://github.com/jbogard/MediatR)
2. Infrastructure專案使用Repositoy Pattern與UnitOfWork Pattern，達成隔離性與可測試性
  
## 能夠運行在 Docker

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

PS. 因container資料庫啟動時需要初始化設定，若API專案的Container啟動失敗，請確認資料庫container已經啟動完成後，再重新啟動API的container

啟動瀏覽器並連線至[swagger](http://localhost:8080/swagger/index.html)，即可查看swagger文件

## 加解密技術應用 (AES/RSA…etc.)
1. 產生JWT Token並使用HmacSha256演算法進行雜湊([可參考位置](https://github.com/BryanYu/CoinDeskAPI/blob/master/CoinDeskAPI/CoinDesk.Utility/JwtTokenGenerator.cs))
2. 在驗證密碼時使用BCrypt進行驗證([可參考位置](https://github.com/BryanYu/CoinDeskAPI/blob/master/CoinDeskAPI/CoinDesk.Domain/CommandHandler/GenerateTokenCommandHandler.cs))