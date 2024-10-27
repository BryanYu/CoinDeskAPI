using System.ComponentModel.DataAnnotations;

namespace CoinDesk.Model.Request;

public class AddCurrencyRequest
{
    /// <summary>
    /// 幣別代碼
    /// </summary>
    [Required(ErrorMessage = "CurrencyCodeRequired")]
    [StringLength(3, ErrorMessage = "CurrencyCodeLenghtMax3")]
    public string CurrencyCode { get; set; }
    
    /// <summary>
    /// 幣別名稱
    /// </summary>
    [Required(ErrorMessage = "CurrencyNameRequired")]
    [StringLength(20, ErrorMessage = "CurrencyNameLengthMax20")]
    public string Name { get; set; }
}