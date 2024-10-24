namespace CoinDesk.Model.Response;

public class GetCurrencyResponse : BasePageResponse
{
    public IEnumerable<CurrencyData> Currencies { get; set; }  
}


public class CurrencyData
{
    public string Name { get; set; }
}

