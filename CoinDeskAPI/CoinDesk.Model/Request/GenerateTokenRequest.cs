using System.ComponentModel.DataAnnotations;

namespace CoinDesk.Model.Request;

public class GenerateTokenRequest
{
    /// <summary>
    /// 帳號
    /// </summary>
    [Required(AllowEmptyStrings = false)]
    public string Account { get; set; }
    
    /// <summary>
    /// 密碼
    /// </summary>
    [Required(AllowEmptyStrings = false)]
    public string Password { get; set; }
    
}