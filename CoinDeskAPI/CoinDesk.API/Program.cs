using CodeDesk.Service.Implements;
using CodeDesk.Service.Interfaces;
using CoinDesk.API.Handler;
using CoinDesk.API.Middleware;
using CoinDesk.Domain.QueryHandler;
using CoinDesk.Infrastructure;
using CoinDesk.Infrastructure.Repository.Base;
using CoinDesk.Infrastructure.Repository.Implements;
using CoinDesk.Infrastructure.Repository.Interfaces;
using CoinDesk.Model.Config;
using Microsoft.EntityFrameworkCore;
using Serilog;
using LoggingHttpMessageHandler = CoinDesk.API.Handler.LoggingHttpMessageHandler;

namespace CoinDesk.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
        Log.Information("Application Startup");

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            var apiXmlPath = Path.Combine(AppContext.BaseDirectory, "CoinDesk.API.xml");
            var modelXmlPath = Path.Combine(AppContext.BaseDirectory, "CoinDesk.Model.xml");
            options.IncludeXmlComments(apiXmlPath);
            options.IncludeXmlComments(modelXmlPath);
        });
        builder.Services.AddDbContext<CurrencyDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("CurrencyDB")));
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<ICurrencyRepository, CurrencyRepository>();
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetCurrencyQueryHandler).Assembly));

        builder.Services.Configure<PaginationConfig>(builder.Configuration.GetSection("PaginationConfig"));
        builder.Services.Configure<CoinDeskConfig>(builder.Configuration.GetSection("CoinDeskConfig"));
        builder.Services.AddProblemDetails();
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddScoped<ICurrencyService, CoinDeskService>();
        builder.Services.AddSerilog();
        builder.Services.AddHttpClient("LoggingHttpClient")
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler())
            .AddHttpMessageHandler((serviceProvider) =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<LoggingHttpMessageHandler>>();
                return new LoggingHttpMessageHandler(logger);
            });
        var app = builder.Build();
        app.UseExceptionHandler();
        app.Use(async (httpContext, next) =>
        {
            var requestId = Guid.NewGuid().ToString();
            httpContext.Request.Headers.TryAdd("RequestId", requestId);
            await next();
            httpContext.Response.Headers.TryAdd("RequestId", requestId);
        });
        app.UseSerilogRequestLogging();
        app.UseMiddleware<HttpLoggingMiddleware>();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        try
        {
            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal("Application terminated unexpectedly: {ex}", ex);
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}