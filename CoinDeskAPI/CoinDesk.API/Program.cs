using CodeDesk.Service.Implements;
using CodeDesk.Service.Interfaces;
using CoinDesk.Domain.QueryHandler;
using CoinDesk.Infrastructure;
using CoinDesk.Infrastructure.Repository.Base;
using CoinDesk.Infrastructure.Repository.Implements;
using CoinDesk.Infrastructure.Repository.Interfaces;
using CoinDesk.Model.Config;
using Microsoft.EntityFrameworkCore;

namespace CoinDesk.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddDbContext<CurrencyDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("CurrencyDB")));
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<ICurrencyRepository, CurrencyRepository>();
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetCurrencyQueryHandler).Assembly));

        builder.Services.Configure<PaginationConfig>(builder.Configuration.GetSection("PaginationConfig"));
        builder.Services.Configure<CoinDeskConfig>(builder.Configuration.GetSection("CoinDeskConfig"));

        builder.Services.AddScoped<ICurrencyService, CoinDeskService>();
        builder.Services.AddHttpClient();
        var app = builder.Build();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}