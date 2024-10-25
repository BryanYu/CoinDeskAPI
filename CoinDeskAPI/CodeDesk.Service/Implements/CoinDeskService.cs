using System.Net.Http.Json;
using System.Text.Json.Nodes;
using CodeDesk.Service.Interfaces;
using CoinDesk.Model.Config;
using CoinDesk.Model.Enum;
using CoinDesk.Model.Response.ThirdParty;
using Microsoft.Extensions.Options;

namespace CodeDesk.Service.Implements;

public class CoinDeskService : ICurrencyService
{
    private readonly IOptions<CoinDeskConfig> _coinDeskConfig;
    private readonly HttpClient _httpClient;

    public CoinDeskService(IHttpClientFactory httpClientFactory, IOptions<CoinDeskConfig> coinDeskConfig)
    {
        _coinDeskConfig = coinDeskConfig;
        this._httpClient = httpClientFactory.CreateClient();
    }

    public async Task<(ThirdPartyApiStatus apiStatus,string updatedTime, Dictionary<string, CurrencyPrice> currencyPrices)> GetCurrencyPriceAsync()
    {
        (ThirdPartyApiStatus status, JsonObject jsonObject) = await GetAsync<JsonObject>(_coinDeskConfig.Value.ApiEndPoint.CurrencyPrice);
        if (status != ThirdPartyApiStatus.Success)
        {
            return (status, string.Empty, null);
        }
        if (jsonObject["time"]?["updatedISO"] == null)
        {
            return (ThirdPartyApiStatus.Failed, string.Empty, null);
        }
        if (!jsonObject.ContainsKey("bpi"))
        {
            return (ThirdPartyApiStatus.Failed, string.Empty, null);
        }

        var updatedTime = jsonObject["time"]["updatedISO"].GetValue<string>();
        var bpis = jsonObject["bpi"].AsObject();

        var result = new Dictionary<string, CurrencyPrice>();
        foreach (var bpi in bpis)
        {
            var currencyCode = bpi.Key;
            var bpiDetail = bpi.Value.AsObject();
            var rate = bpiDetail.ContainsKey("rate_float") ? bpiDetail["rate_float"].GetValue<decimal>() : 0m;
            var currencyPrice = new CurrencyPrice
            {
                CurrencyCode = currencyCode,
                Rate = rate
            };
            result.TryAdd(currencyCode, currencyPrice);
        }
        return (ThirdPartyApiStatus.Success, updatedTime, result);
    }

    private async Task<(ThirdPartyApiStatus apiStatus, T)> GetAsync<T>(string url) where T : class
    {
        var response = await this._httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            return (ThirdPartyApiStatus.Failed, default(T));
        }
        var result = await response.Content.ReadFromJsonAsync<T>();
        if (result == null)
        {
            return (ThirdPartyApiStatus.Failed, default(T));
        }
        return (ThirdPartyApiStatus.Success, result);
    }
    
    
}