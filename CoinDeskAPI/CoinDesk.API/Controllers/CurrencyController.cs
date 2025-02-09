﻿using CoinDesk.Model.Command;
using CoinDesk.Model.Query;
using CoinDesk.Model.Request;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoinDesk.API.Controllers;

/// <summary>
/// 幣別資訊API
/// </summary>
[Authorize]
[ApiController]
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
        var response = await _mediator.Send(new GetCurrencyQuery
        {
            PageSize = request.PageSize,
            PageNumber = request.PageNumber
        });
        return Ok(response);
    }

    /// <summary>
    /// 新增幣別
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("")]
    public async Task<IActionResult> AddCurrency([FromBody] AddCurrencyRequest request)
    {
        var result = await _mediator.Send(new AddCurrencyCommand
        {
            Name = request.Name,
            CurrencyCode = request.CurrencyCode
        });
        return Ok(result);
    }

    /// <summary>
    /// 更新幣別
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> UpdateCurrency([FromRoute] Guid id, [FromBody] UpdateCurrencyRequest request)
    {
        var result = await _mediator.Send(new UpdateCurrencyCommand
        {
            Id = id,
            Name = request.Name
        });
        return Ok(result);
    }

    /// <summary>
    /// 刪除幣別
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeleteCurrency([FromRoute] Guid id)
    {
        var result = await _mediator.Send(new DeleteCurrencyCommand
        {
            Id = id
        });
        return Ok(result);
    }

    /// <summary>
    /// 測試Exception
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("Exception")]
    public async Task<IActionResult> Exception()
    {
        throw new ArgumentException("Argument Exception");
    }
}