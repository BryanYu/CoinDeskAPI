using System.Net;
using CodeDesk.Service.Implements;
using CoinDesk.Model.Config;
using CoinDesk.Model.Enum;
using CoinDesk.Model.Response.ThirdParty;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;

namespace CoinDeskTests.ServiceTests;

public class CoinDeskServiceTests
{
    private Mock<IHttpClientFactory> _httpClientFactoryMock;
    private CoinDeskService _coinDeskService;
    private IOptions<CoinDeskConfig> _coinDeskConfig;
    
    [SetUp]
    public void SetUp()
    {
        _httpClientFactoryMock = new Mock<IHttpClientFactory>();
        _coinDeskConfig = Options.Create(new CoinDeskConfig
        {
            ApiEndPoint = new ApiEndPoint
            {
                CurrencyPrice = "https://www.google.com"
            }
        });
    }


    [Test]
    public async Task CoinDeskService_GetCurrencyPriceAsync_ReturnData()
    {
        // arrange
        var expectedContent = this.GetCoinDeskApiSuccessResponse();
        var expectedUpdateTime = "2024-10-26T16:05:45+00:00";
        var handlerMock = GenerateHttpMessageHandlerMock(HttpStatusCode.OK, expectedContent);
        var httpClient = new HttpClient(handlerMock.Object);
        _httpClientFactoryMock.Setup(item => item.CreateClient(It.IsAny<string>())).Returns(httpClient);
        var expectedCurrencyPrice = new Dictionary<string, CurrencyPrice>
        {
            ["USD"] = new CurrencyPrice { CurrencyCode = "USD", Rate = 1m },
            ["GBP"] = new CurrencyPrice { CurrencyCode = "GBP", Rate = 2m },
            ["EUR"] = new CurrencyPrice { CurrencyCode = "EUR", Rate = 3m },
        };
        var coinDeskService = new CoinDeskService(_httpClientFactoryMock.Object, _coinDeskConfig);

        // actual
        var actual = await coinDeskService.GetCurrencyPriceAsync();
        
        // assert
        actual.apiStatus.Should().Be(ThirdPartyApiStatus.Success);
        actual.currencyPrices.Should().BeEquivalentTo(expectedCurrencyPrice);
        actual.updatedTime.Should().NotBeEmpty();
        actual.updatedTime.Should().Be(expectedUpdateTime);
    }

    [Test]
    public async Task CoinDeskService_GetCurrencyPriceAsync_WhenTimeUpdatedISONotFound_ReturnFailed()
    {
        // arrange
        var expectedContent = this.GetCoinDeskApiTimeUpdatedISONotFoundResponse();
        var handlerMock = GenerateHttpMessageHandlerMock(HttpStatusCode.OK, expectedContent);
        var httpClient = new HttpClient(handlerMock.Object);
        _httpClientFactoryMock.Setup(item => item.CreateClient(It.IsAny<string>())).Returns(httpClient);
        var coinDeskService = new CoinDeskService(_httpClientFactoryMock.Object, _coinDeskConfig);
        // actual
        var actual = await coinDeskService.GetCurrencyPriceAsync();
        // assert
        actual.apiStatus.Should().Be(ThirdPartyApiStatus.Failed);
    }

    [Test]
    public async Task CoinDeskService_GetCurrencyPriceAsync_WhenBpiNotFound_ReturnFailed()
    {
        // arrange
        var expectedContent = this.GetCoinDeskApiBpiNotFoundResponse();
        var handlerMock = GenerateHttpMessageHandlerMock(HttpStatusCode.OK, expectedContent);
        var httpClient = new HttpClient(handlerMock.Object);
        _httpClientFactoryMock.Setup(item => item.CreateClient(It.IsAny<string>())).Returns(httpClient);
        var coinDeskService = new CoinDeskService(_httpClientFactoryMock.Object, _coinDeskConfig);
        // actual
        var actual = await coinDeskService.GetCurrencyPriceAsync();
        // assert
        actual.apiStatus.Should().Be(ThirdPartyApiStatus.Failed);
    }

    [Test]
    public async Task CoinDeskService_GetCurrencyPriceAsync_WhenApiFailed_ReturnFailed()
    {
        // arrange
        var expectedContent = string.Empty;
        var handlerMock = GenerateHttpMessageHandlerMock(HttpStatusCode.Forbidden, expectedContent);
        var httpClient = new HttpClient(handlerMock.Object);
        _httpClientFactoryMock.Setup(item => item.CreateClient(It.IsAny<string>())).Returns(httpClient);
        var coinDeskService = new CoinDeskService(_httpClientFactoryMock.Object, _coinDeskConfig);
        // actual
        var actual = await coinDeskService.GetCurrencyPriceAsync();
        // assert
        actual.apiStatus.Should().Be(ThirdPartyApiStatus.Failed);
    }
    
    [Test]
    public async Task CoinDeskService_GetCurrencyPriceAsync_WhenResponseContentIsEmpty_ReturnFailed()
    {
        // arrange
        var expectedContent = string.Empty;
        var handlerMock = GenerateHttpMessageHandlerMock(HttpStatusCode.OK, expectedContent);
        var httpClient = new HttpClient(handlerMock.Object);
        _httpClientFactoryMock.Setup(item => item.CreateClient(It.IsAny<string>())).Returns(httpClient);
        var coinDeskService = new CoinDeskService(_httpClientFactoryMock.Object, _coinDeskConfig);
        // actual
        var actual = await coinDeskService.GetCurrencyPriceAsync();
        // assert
        actual.apiStatus.Should().Be(ThirdPartyApiStatus.Failed);
    }
    
    private Mock<HttpMessageHandler> GenerateHttpMessageHandlerMock(HttpStatusCode httpStatusCode, string expectedContent)
    {
        var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = httpStatusCode,
                Content = new StringContent(expectedContent),
            })
            .Verifiable();
        return handlerMock;
    }
    private string GetCoinDeskApiSuccessResponse()
    {
        return @"{
                  ""time"": {
                    ""updated"": ""Oct 26, 2024 04:05:45 UTC"",
                    ""updatedISO"": ""2024-10-26T16:05:45+00:00"",
                    ""updateduk"": ""Oct 26, 2024 at 05:05 BST""
                  },
                  ""disclaimer"": ""This data was produced from the CoinDesk Bitcoin Price Index (USD). Non-USD currency data converted using hourly conversion rate from openexchangerates.org"",
                  ""chartName"": ""Bitcoin"",
                  ""bpi"": {
                    ""USD"": {
                      ""code"": ""USD"",
                      ""symbol"": ""&#36;"",
                      ""rate"": ""66,833.183"",
                      ""description"": ""United States Dollar"",
                      ""rate_float"": 1
                    },
                    ""GBP"": {
                      ""code"": ""GBP"",
                      ""symbol"": ""&pound;"",
                      ""rate"": ""51,564.808"",
                      ""description"": ""British Pound Sterling"",
                      ""rate_float"": 2
                    },
                    ""EUR"": {
                      ""code"": ""EUR"",
                      ""symbol"": ""&euro;"",
                      ""rate"": ""61,879.107"",
                      ""description"": ""Euro"",
                      ""rate_float"": 3
                    }
                  }
}";
    }

    private string GetCoinDeskApiTimeUpdatedISONotFoundResponse()
    {
        return @"{
                  ""disclaimer"": ""This data was produced from the CoinDesk Bitcoin Price Index (USD). Non-USD currency data converted using hourly conversion rate from openexchangerates.org"",
                  ""chartName"": ""Bitcoin"",
                  ""bpi"": {
                    ""USD"": {
                      ""code"": ""USD"",
                      ""symbol"": ""&#36;"",
                      ""rate"": ""66,833.183"",
                      ""description"": ""United States Dollar"",
                      ""rate_float"": 1
                    },
                    ""GBP"": {
                      ""code"": ""GBP"",
                      ""symbol"": ""&pound;"",
                      ""rate"": ""51,564.808"",
                      ""description"": ""British Pound Sterling"",
                      ""rate_float"": 2
                    },
                    ""EUR"": {
                      ""code"": ""EUR"",
                      ""symbol"": ""&euro;"",
                      ""rate"": ""61,879.107"",
                      ""description"": ""Euro"",
                      ""rate_float"": 3
                    }
                  }
}";
    }

    private string GetCoinDeskApiBpiNotFoundResponse()
    {
        return @"{
                  ""time"": {
                    ""updated"": ""Oct 26, 2024 04:05:45 UTC"",
                    ""updatedISO"": ""2024-10-26T16:05:45+00:00"",
                    ""updateduk"": ""Oct 26, 2024 at 05:05 BST""
                  },
                  ""disclaimer"": ""This data was produced from the CoinDesk Bitcoin Price Index (USD). Non-USD currency data converted using hourly conversion rate from openexchangerates.org"",
                  ""chartName"": ""Bitcoin""
}";
    }
}