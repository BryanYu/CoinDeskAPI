using System.Text.Json;
using System.Text.Json.Serialization;
using CoinDesk.API.ActionFilter;
using Microsoft.AspNetCore.Mvc;

namespace CoinDesk.API.Extension;

public static class ControllerExtension
{
    public static IServiceCollection AddCustomController(this IServiceCollection services)
    {

        services.AddControllers(options =>
            {
                options.Filters.Add<ModelValidateActionFilter>();
                options.Filters.Add<GlobalResponseActionFilter>();
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });
        services.Configure<ApiBehaviorOptions>(item => item.SuppressModelStateInvalidFilter = true);
        return services;

    }
}