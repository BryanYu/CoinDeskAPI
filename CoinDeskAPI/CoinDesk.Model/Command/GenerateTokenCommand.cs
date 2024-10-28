using CoinDesk.Model.Response;
using MediatR;

namespace CoinDesk.Model.Command;

public class GenerateTokenCommand : IRequest<HandlerResponse>
{
    /// <summary>
    /// 帳號
    /// </summary>
    public string Account { get; set; }
    
    /// <summary>
    /// 密碼
    /// </summary>
    public string Password { get; set; }
}