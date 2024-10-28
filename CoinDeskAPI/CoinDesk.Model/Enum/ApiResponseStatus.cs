using System.ComponentModel;
using CoinDesk.Model.Attribute;

namespace CoinDesk.Model.Enum;

public enum ApiResponseStatus
{
    /// <summary>
    /// 執行成功
    /// </summary>
    [LocalizeKey("Success")]
    Success = 1,
    
    /// <summary>
    /// 模型驗證錯誤
    /// </summary>
    [LocalizeKey("ModelValidError")]
    ModelValidError = -1000,
    
    /// <summary>
    /// 使用者不存在
    /// </summary>
    [LocalizeKey("UserNotExist")]
    UserNotExist = -2000,
    
    /// <summary>
    /// 使用者密碼錯誤
    /// </summary>
    [LocalizeKey("UserPasswordError")]
    UserPasswordError = -2001,
    
    /// <summary>
    /// 第三方API發生錯誤
    /// </summary>
    [LocalizeKey("ThirdPartyApiError")]
    ThirdPartyApiError = -3000,
    
    
    /// <summary>
    /// 貨幣資料已存在
    /// </summary>
    [LocalizeKey("CurrencyExist")]
    CurrencyExist = -4000,
    
    /// <summary>
    /// 貨幣資料不存在
    /// </summary>
    [LocalizeKey("CurrencyNotExist")]
    CurrencyNotExist = -4001,
    
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