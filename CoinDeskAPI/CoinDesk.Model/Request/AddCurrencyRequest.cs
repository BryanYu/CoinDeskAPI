using System.ComponentModel.DataAnnotations;

namespace CoinDesk.Model.Request;

public class AddCurrencyRequest
{
    /// <summary>
    /// 幣別代碼
    /// </summary>
    [Required]
    [StringLength(3)]
    public string CurrencyCode { get; set; }
    
    /// <summary>
    /// 幣別名稱
    /// </summary>
    [Required]
    [StringLength(20)]
    public string Name { get; set; }
}