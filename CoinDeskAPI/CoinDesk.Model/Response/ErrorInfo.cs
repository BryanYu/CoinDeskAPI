namespace CoinDesk.Model.Response;

public class ErrorInfo
{
    /// <summary>
    /// 欄位
    /// </summary>
    public string Field { get; set; }
    
    /// <summary>
    /// 錯誤訊息
    /// </summary>
    public string Message { get; set; }
}