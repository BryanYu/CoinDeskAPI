using CodeDesk.Service.Interfaces;
using CoinDesk.Domain.QueryHandler;
using CoinDesk.Infrastructure.Model;
using CoinDesk.Infrastructure.Repository.Base;
using CoinDesk.Infrastructure.Repository.Interfaces;
using CoinDesk.Model.Enum;
using CoinDesk.Model.Query;
using CoinDesk.Model.Response;
using CoinDesk.Model.Response.ThirdParty;
using FluentAssertions;
using Moq;

namespace CoinDeskTests.QueryHandlerTests;

public class GetCurrencyQueryHandlerTests
{
    private GetCurrencyQueryHandler _getCurrencyQueryHandler;
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private Mock<ICurrencyRepository> _currencyRepositoryMock;
    private Mock<ICurrencyService> _currencyServiceMock;
    
    [SetUp]
    public void SetUp()
    {
        _currencyRepositoryMock = new Mock<ICurrencyRepository>();
        _currencyServiceMock = new Mock<ICurrencyService>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _unitOfWorkMock.Setup(item => item.CurrencyRepository).Returns(_currencyRepositoryMock.Object);
        _unitOfWorkMock.Setup(item => item.SaveChangesAsync()).ReturnsAsync(1);
        _getCurrencyQueryHandler = new GetCurrencyQueryHandler(_unitOfWorkMock.Object, _currencyServiceMock.Object);
    }

    [Test]
    public async Task GetCurrencyQueryHandler_GetCurrencyPrice_WhenApiFailed_ReturnEmptyResponse()
    {
        // arrange 
        var returnCurrencyPriceMock = ValueTuple.Create(ThirdPartyApiStatus.Failed, string.Empty,
            new Dictionary<string, CurrencyPrice>());
        this._currencyServiceMock.Setup(item => item.GetCurrencyPriceAsync()).ReturnsAsync(returnCurrencyPriceMock);
        var expected = new PagedResultResponse<CurrencyResponse>();

        // actual 
        var actual = await _getCurrencyQueryHandler.Handle(new GetCurrencyQuery(), CancellationToken.None);
        
        // assert
        expected.Should().BeEquivalentTo(actual);
    }

    [Test]
    public async Task GetCurrencyQueryHandler_GetCurrencyPrice_WhenApiSuccess_ReturnCurrencyResponse()
    {
        // arrange
        var returnCurrencyPriceMock = this.GenerateCurrencyPriceMock();
        var usdId = Guid.NewGuid();
        var gbpId = Guid.NewGuid();
        var eurId = Guid.NewGuid();
        var pagedQueryResultMock = new PagedQueryResult<Currency>
        {
            Items = new List<Currency>
            {
                new Currency { Id = usdId, Name = "美金", CurrencyCode = "USD" },
                new Currency { Id = gbpId, Name = "英鎊", CurrencyCode = "GBP" },
                new Currency { Id = eurId, Name = "歐元", CurrencyCode = "EUR" },
            },
            TotalRecords = 3
        };
        _currencyServiceMock.Setup(item => item.GetCurrencyPriceAsync()).ReturnsAsync(returnCurrencyPriceMock);
        _currencyRepositoryMock.Setup(item => item.GetPagingAsync(null,
                It.IsAny<Func<IQueryable<Currency>, IOrderedQueryable<Currency>>>(), It.IsAny<PaginationParameter>()))
            .ReturnsAsync(pagedQueryResultMock);
        var expected = new PagedResultResponse<CurrencyResponse>
        {
            Pagination = new Pagination
            {
                TotalRecords = 3,
                PageNumber = 1,
                PageSize = 10,
            },
            Data = new CurrencyResponse
            {
                Currencies = new List<CurrencyDetailResponse>
                {
                    new CurrencyDetailResponse { Id = usdId, Name = "美金", CurrencyCode = "USD", Rate = 1m },
                    new CurrencyDetailResponse { Id = gbpId, Name = "英鎊", CurrencyCode = "GBP", Rate = 2m },
                    new CurrencyDetailResponse { Id = eurId, Name = "歐元", CurrencyCode = "EUR", Rate = 3m },
                },
                UpdatedTime = "2024/10/27 00:00:00"
            }
        };
        
        // actual 
        var actual = await _getCurrencyQueryHandler.Handle(new GetCurrencyQuery
        {
            PageNumber = 1,
            PageSize = 10
        }, CancellationToken.None);

        // assert 
        actual.Should().BeEquivalentTo(expected);
    }


    private (ThirdPartyApiStatus apiStatus, string updatedTime, Dictionary<string, CurrencyPrice> currencyPrices)
        GenerateCurrencyPriceMock()
    {
        return ValueTuple.Create(ThirdPartyApiStatus.Success, "2024-10-26T16:00:00+00:00",
            new Dictionary<string, CurrencyPrice>
            {
                ["USD"] = new CurrencyPrice
                {
                    CurrencyCode = "USD",
                    Rate = 1m,
                },
                ["GBP"] = new CurrencyPrice
                {
                    CurrencyCode = "GBP",
                    Rate = 2m,
                },
                ["EUR"] = new CurrencyPrice
                {
                    CurrencyCode = "EUR",
                    Rate = 3m,
                }
            }); 
    }
}