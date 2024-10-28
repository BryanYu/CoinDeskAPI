using CoinDesk.Model.Command;
using CoinDesk.Model.Enum;
using CoinDesk.Model.Request;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CoinDesk.API.Controllers;

/// <summary>
/// Token Controller
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TokenController : ControllerBase
{
    private readonly IMediator _mediator;

    public TokenController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    /// <summary>
    /// 取得Token
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("")]
    public async Task<IActionResult> GenerateToken([FromBody] GenerateTokenRequest request)
    {
        var result = await _mediator.Send(new GenerateTokenCommand
        {
            Account = request.Account,
            Password = request.Password
        });
        if (result.Status != ApiResponseStatus.Success)
        {
            return new BadRequestObjectResult(result);
        }
        return Ok(result);
    }
}