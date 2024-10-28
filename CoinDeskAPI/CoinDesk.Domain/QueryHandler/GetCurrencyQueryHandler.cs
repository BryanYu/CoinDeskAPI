using CodeDesk.Service.Interfaces;
using CoinDesk.Infrastructure.Model;
using CoinDesk.Infrastructure.Repository.Base;
using CoinDesk.Model.Enum;
using CoinDesk.Model.Query;
using CoinDesk.Model.Response;
using CoinDesk.Model.Response.ThirdParty;
using MediatR;
using Microsoft.Extensions.Localization;

namespace CoinDesk.Domain.QueryHandler;

public class GetCurrencyQueryHandler : IRequestHandler<GetCurrencyQuery, HandlerResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrencyService _currencyService;
    private readonly ILocalizeService _localizeService;
    
    public GetCurrencyQueryHandler(IUnitOfWork unitOfWork, ICurrencyService currencyService, ILocalizeService localizeService)
    {
        _unitOfWork = unitOfWork;
        _currencyService = currencyService;
        _localizeService = localizeService;
    }
    
    public async Task<HandlerResponse> Handle(GetCurrencyQuery request, CancellationToken cancellationToken)
    {
        var currencyResult = await this._currencyService.GetCurrencyPriceAsync();
        if (currencyResult.apiStatus != ThirdPartyApiStatus.Success)
        {
            return new HandlerResponse
            {
                Status = ApiResponseStatus.ThirdPartyApiError
            };
        }
        var pagingParameter = new PaginationParameter
        {
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
        var queryResult = await _unitOfWork.CurrencyRepository.GetPagingAsync(
            orderBy: condition => condition.OrderByDescending(item => item.CurrencyCode)
            , pagingParameter: pagingParameter);

        var currencyDetails = queryResult.Items.Select(item =>
        {
            var rate = 0m;
            if (currencyResult.currencyPrices.TryGetValue(item.CurrencyCode, out var currencyPrice))
            {
                rate = currencyPrice.Rate;
            }

            return new CurrencyDetailResponse
            {
                Id = item.Id,
                Name = _localizeService.GetLocalizedString(LocalizeType.Currency, item.CurrencyCode),
                CurrencyCode = item.CurrencyCode,
                Rate = rate
            };
        });
        
        var currencyCodes = queryResult.Items.Select(item => item.CurrencyCode);
        var updatedTime = ConvertUpdatedTime(currencyCodes, currencyResult.updatedTime, currencyResult.currencyPrices);
        return new HandlerResponse
        {
            Status = ApiResponseStatus.Success,
            Data = new PagedResultResponse<CurrencyResponse>
            {
                Pagination = new Pagination
                {
                    TotalRecords = queryResult.TotalRecords,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize,
                },
                Data = new CurrencyResponse
                {
                    Currencies = currencyDetails,
                    UpdatedTime = updatedTime
                }
            }
        };
    }

    private string ConvertUpdatedTime(IEnumerable<string> currencyCodes, string updatedTime, Dictionary<string, CurrencyPrice> currencyPrices)
    {
        if (currencyPrices.Keys.Any(currencyCodes.Contains) && DateTimeOffset.TryParse(updatedTime, out var parseUpdatedTime))
        {
            return parseUpdatedTime.ToOffset(TimeSpan.FromHours(8)).DateTime.ToString("yyyy/MM/dd HH:mm:ss");
        }
        return string.Empty;
    }
}