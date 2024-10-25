using CoinDesk.Model.Enum;

namespace CoinDesk.Model.Response.ThirdParty;

public class CurrencyPrice
{
    /// <summary>
    /// 幣別代碼
    /// </summary>
    public string CurrencyCode { get; set; }
    
    /// <summary>
    /// 匯率
    /// </summary>
    public decimal Rate { get; set; }
}