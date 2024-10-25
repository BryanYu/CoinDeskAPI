namespace CoinDesk.Model.Config;

public class CoinDeskConfig
{
    public ApiEndPoint ApiEndPoint { get; set; }
}
public class ApiEndPoint
{
    public string CurrencyPrice { get; set; }
} 