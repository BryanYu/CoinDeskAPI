using CoinDesk.Model.Query;
using CoinDesk.Model.Request;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CoinDesk.API.Controllers;

/// <summary>
/// 幣別資訊API
/// </summary>
[Route("api/[controller]")]
public class CurrencyController : ControllerBase
{
    private readonly IMediator _mediator;

    public CurrencyController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// 取得幣別
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetCurrency([FromQuery] GetCurrencyRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        var response = await _mediator.Send(new GetCurrencyQuery
        {
            PageSize = request.PageSize,
            PageNumber = request.PageNumber
        });
        return Ok(response);
    }
}