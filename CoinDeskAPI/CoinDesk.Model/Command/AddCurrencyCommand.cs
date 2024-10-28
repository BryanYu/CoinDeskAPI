using System.Runtime.CompilerServices;
using CoinDesk.Model.Response;
using MediatR;

namespace CoinDesk.Model.Command;

public class AddCurrencyCommand : IRequest<HandlerResponse>
{
    /// <summary>
    /// 幣別代碼
    /// </summary>
    public string CurrencyCode { get; set; }

    /// <summary>
    /// 幣別名稱
    /// </summary>
    public string Name { get; set; }
}