using MediatR;

namespace CoinDesk.Model.Command;

public class DeleteCurrencyCommand : IRequest<bool>
{
    /// <summary>
    /// 幣別Id
    /// </summary>
    public Guid Id { get; set; }
}