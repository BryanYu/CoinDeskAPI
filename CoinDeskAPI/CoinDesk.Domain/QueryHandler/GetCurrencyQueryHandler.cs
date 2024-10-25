using CoinDesk.Infrastructure.Model;
using CoinDesk.Infrastructure.Repository.Base;
using CoinDesk.Model.Query;
using CoinDesk.Model.Response;
using MediatR;

namespace CoinDesk.Domain.QueryHandler;

public class GetCurrencyQueryHandler : IRequestHandler<GetCurrencyQuery, PagedResultResponse<CurrencyResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCurrencyQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<PagedResultResponse<CurrencyResponse>> Handle(GetCurrencyQuery request, CancellationToken cancellationToken)
    {
        var pagingParameter = new PaginationParameter
        {
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
        var result = await _unitOfWork.CurrencyRepository.GetPagingAsync(pagingParameter: pagingParameter);
        return new PagedResultResponse<CurrencyResponse>
        {
            Pagination = new Pagination
            {
                TotalRecords = result.TotalRecords,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
            },
            Items = result.Items.Select(item => new CurrencyResponse
            {
                Name = item.Name,
                CurrencyCode = item.CurrencyCode.ToString()
            })
        };
    }
}