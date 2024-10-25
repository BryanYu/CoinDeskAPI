namespace CoinDesk.Model.Response;

public class CurrencyResponse
{
    /// <summary>
    /// 幣別名稱
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// 幣別代碼
    /// </summary>
    public string CurrencyCode { get; set; }
    
    /// <summary>
    /// 匯率
    /// </summary>
    public decimal Rate { get; set; }
    
    /// <summary>
    /// 匯率更新時間
    /// </summary>
    public string UpdateTime { get; set; }
}

