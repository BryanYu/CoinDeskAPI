using MediatR;

namespace CoinDesk.Model.Command;

public class UpdateCurrencyCommand : IRequest<bool>
{
    /// <summary>
    /// 幣別Id
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// 幣別名稱
    /// </summary>
    public string Name { get; set; }
    
}