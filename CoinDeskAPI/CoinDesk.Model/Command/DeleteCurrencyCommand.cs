using CoinDesk.Model.Response;
using MediatR;

namespace CoinDesk.Model.Command;

public class DeleteCurrencyCommand : IRequest<HandlerResponse>
{
    /// <summary>
    /// 幣別Id
    /// </summary>
    public Guid Id { get; set; }
}