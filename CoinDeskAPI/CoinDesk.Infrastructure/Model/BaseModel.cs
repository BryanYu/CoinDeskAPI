using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace CoinDesk.Infrastructure.Model;

public class BaseModel
{
    /// <summary>
    /// 建立時間
    /// </summary>
    [Required]
    [Comment("建立時間")]
    public DateTime CreatedTime { get; set; }
    
    /// <summary>
    /// 修改時間
    /// </summary>
    [Required]
    [Comment("修改時間")]
    public DateTime? UpdateTime { get; set; }
}