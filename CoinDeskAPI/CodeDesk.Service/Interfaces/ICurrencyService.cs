using CoinDesk.Model.Enum;
using CoinDesk.Model.Response.ThirdParty;

namespace CodeDesk.Service.Interfaces;

public interface ICurrencyService
{
    Task<(ThirdPartyApiStatus apiStatus, string updatedTime, Dictionary<string, CurrencyPrice> currencyPrices)>
        GetCurrencyPriceAsync();
}