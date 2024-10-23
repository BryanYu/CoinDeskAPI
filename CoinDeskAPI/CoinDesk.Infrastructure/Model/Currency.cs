using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CoinDesk.Model.Enum;
using Microsoft.EntityFrameworkCore;

namespace CoinDesk.Infrastructure.Model;

public class Currency : BaseModel
{
    /// <summary>
    /// 幣別Id
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Comment("幣別Id")]
    public Guid Id { get; set; }

    /// <summary>
    /// 幣別名稱
    /// </summary>
    [StringLength(20)]
    [Required]
    [Comment("幣別名稱")]
    public string Name { get; set; }

    /// <summary>
    /// 幣別代碼
    /// </summary>
    [Required]
    [Comment("幣別代碼")]
    public CurrencyCode CurrencyCode { get; set; }

}