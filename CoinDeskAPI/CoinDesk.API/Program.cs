using CodeDesk.Service.Implements;
using CodeDesk.Service.Interfaces;
using CoinDesk.API.Extension;
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

namespace CoinDesk.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
        Log.Information($"Application Startup, Environment:{builder.Environment.EnvironmentName}");
        
        builder.Services.AddControllers();
        builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddDbContext<CurrencyDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("CurrencyDB")));
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetCurrencyQueryHandler).Assembly));
        builder.Services.AddProblemDetails();
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddSerilog();
        
        builder.Services.AddCustomSwaagerGen();
        builder.Services.AddCustomRepositoy();
        builder.Services.AddCustomConfigure(builder.Configuration);
        builder.Services.AddCustomService();
        builder.Services.AddCustomHttpClient();
        
        var app = builder.Build();
        app.UseCustomRequestLocalization();
        app.UseExceptionHandler();
        app.UseCustomRequestIdOnHeader();
        app.UseSerilogRequestLogging();
        app.UseMiddleware<HttpLoggingMiddleware>();
        if (app.Environment.IsDevelopment() || app.Environment.IsDocker())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        if (app.Environment.IsDocker())
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<CurrencyDbContext>();
            dbContext.Database.Migrate();
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