using System.ComponentModel.DataAnnotations;

namespace CoinDesk.Model.Request;

public class UpdateCurrencyRequest
{
    /// <summary>
    /// 幣別名稱
    /// </summary>
    [Required(ErrorMessage = "CurrencyNameRequired")]
    [StringLength(20, ErrorMessage = "CurrencyNameLengthMax20")]
    public string Name { get; set; }
}