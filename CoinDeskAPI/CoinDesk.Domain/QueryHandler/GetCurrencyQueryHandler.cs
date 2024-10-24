using CoinDesk.Infrastructure.Model;
using CoinDesk.Infrastructure.Repository.Base;
using CoinDesk.Model.Query;
using CoinDesk.Model.Response;
using MediatR;

namespace CoinDesk.Domain.QueryHandler;

public class GetCurrencyQueryHandler : IRequestHandler<GetCurrencyQuery, GetCurrencyResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCurrencyQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<GetCurrencyResponse> Handle(GetCurrencyQuery request, CancellationToken cancellationToken)
    {
        var pagingParameter = new PagingParameter
        {
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
        var result = await _unitOfWork.CurrencyRepository.GetPagingAsync(pagingParameter: pagingParameter);
        return new GetCurrencyResponse
        {
            Currencies = result.Items.Select(item => new CurrencyData
            {

            }),
            TotalRecords = result.TotalRecords,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}