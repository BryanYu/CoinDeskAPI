using System.ComponentModel;
using CoinDesk.Model.Attribute;

namespace CoinDesk.Model.Enum;

public enum ApiResponseStatus
{
    /// <summary>
    /// 執行成功
    /// </summary>
    [LocalizeKey("Success")]
    Success = 0,
    
    /// <summary>
    /// 模型驗證錯誤
    /// </summary>
    [LocalizeKey("ModelValidError")]
    ModelValidError = -1000,
    
    /// <summary>
    /// 伺服器錯誤
    /// </summary>
    [LocalizeKey("InternalServerError")]
    InternalServerError = -9998,
    
    /// <summary>
    /// 通用錯誤
    /// </summary>
    [LocalizeKey("Failed")]
    Failed = -9999
}