using CoinDesk.Model.Enum;

namespace CoinDesk.Model.Response;

public class ApiResponse<T>
{
    /// <summary>
    /// Api回傳狀態
    /// </summary>
    public ApiResponseStatus Status { get; set; }
    
    /// <summary>
    /// 訊息
    /// </summary>
    public string Message { get; set; }
    
    /// <summary>
    /// 發生錯誤時的詳細資訊
    /// </summary>
    public List<ErrorInfo> Errors { get; set; }
    
    /// <summary>
    /// 回傳的資料
    /// </summary>
    public T Result { get; set; }
}