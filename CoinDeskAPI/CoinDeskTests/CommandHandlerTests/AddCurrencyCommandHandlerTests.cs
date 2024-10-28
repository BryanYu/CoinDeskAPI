using System.Linq.Expressions;
using System.Reflection.PortableExecutable;
using CoinDesk.Domain.CommandHandler;
using CoinDesk.Infrastructure.Model;
using CoinDesk.Infrastructure.Repository.Base;
using CoinDesk.Infrastructure.Repository.Interfaces;
using CoinDesk.Model.Command;
using CoinDesk.Model.Enum;
using CoinDesk.Model.Response;
using FluentAssertions;
using Moq;

namespace CoinDeskTests.CommandHandlerTests;

[TestFixture]
public class AddCurrencyCommandHandlerTests
{
    private AddCurrencyCommandHandler _addCurrencyCommandHandler;
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private Mock<ICurrencyRepository> _currencyRepositoryMock;
    
    [SetUp]
    public void SetUp()
    {
        _currencyRepositoryMock = new Mock<ICurrencyRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _unitOfWorkMock.Setup(item => item.CurrencyRepository).Returns(_currencyRepositoryMock.Object);
        _unitOfWorkMock.Setup(item => item.SaveChangesAsync()).ReturnsAsync(1);
        _addCurrencyCommandHandler = new AddCurrencyCommandHandler(_unitOfWorkMock.Object);
    }
    
    [Test]
    public async Task AddCurrencyCommandHandler_AddExistCurrency_ReturnCurrencyExist()
    {
        // arrange
        this._currencyRepositoryMock.Setup(item => item.AnyAsync(It.IsAny<Expression<Func<Currency, bool>>>()))
            .ReturnsAsync(true);
        var expected = new HandlerResponse
        {
            Status = ApiResponseStatus.CurrencyExist
        };
        
        // actual 
        var actual = await _addCurrencyCommandHandler.Handle(new AddCurrencyCommand
        {
            CurrencyCode = "USD",
            Name = "美金"
        }, CancellationToken.None);
        
        // assert 
        expected.Should().BeEquivalentTo(actual);
    }

    [Test]
    public async Task AddCurrencyCommandHandler_AddNonExistCurrency_ReturnSuccess()
    {
        // arrange
        this._currencyRepositoryMock.Setup(item => item.AnyAsync(It.IsAny<Expression<Func<Currency, bool>>>()))
            .ReturnsAsync(false);
        var expected = new HandlerResponse
        {
            Status = ApiResponseStatus.Success
        };
        
        // actual
        var actual = await _addCurrencyCommandHandler.Handle(new AddCurrencyCommand
        {
            CurrencyCode = "USD",
            Name = "美金"
        }, CancellationToken.None);

        // assert
        expected.Should().BeEquivalentTo(actual);
        this._currencyRepositoryMock.Verify(item => item.AddAsync(It.IsAny<Currency>()), Times.Once);
        this._unitOfWorkMock.Verify(item => item.SaveChangesAsync(), Times.Once);
    }
}