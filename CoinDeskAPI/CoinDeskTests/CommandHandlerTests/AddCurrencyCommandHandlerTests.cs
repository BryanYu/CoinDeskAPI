using System.Linq.Expressions;
using System.Reflection.PortableExecutable;
using CoinDesk.Domain.CommandHandler;
using CoinDesk.Infrastructure.Model;
using CoinDesk.Infrastructure.Repository.Base;
using CoinDesk.Infrastructure.Repository.Interfaces;
using CoinDesk.Model.Command;
using FluentAssertions;
using Moq;

namespace CoinDeskTests.CommandHandlerTests;

[TestFixture]
public class AddCurrencyCommandHandlerTests
{
    private AddCurrencyCommandHandler _addCurrencyCommandHandler;
    private Mock<IUnitOfWork> _unitWorkMock;
    private Mock<ICurrencyRepository> _currencyRepositoryMock;
    
    [SetUp]
    public void SetUp()
    {
        _currencyRepositoryMock = new Mock<ICurrencyRepository>();
        _unitWorkMock = new Mock<IUnitOfWork>();
        _unitWorkMock.Setup(item => item.CurrencyRepository).Returns(_currencyRepositoryMock.Object);
        _unitWorkMock.Setup(item => item.SaveChangesAsync()).ReturnsAsync(1);
        _addCurrencyCommandHandler = new AddCurrencyCommandHandler(_unitWorkMock.Object);
    }
    
    [Test]
    public async Task AddCurrencyCommandHandler_AddExistCurrency_ReturnFalse()
    {
        // arrange
        this._currencyRepositoryMock.Setup(item => item.AnyAsync(It.IsAny<Expression<Func<Currency, bool>>>()))
            .ReturnsAsync(true);
        var expected = false;
        
        // actual 
        var actual = await _addCurrencyCommandHandler.Handle(new AddCurrencyCommand
        {
            CurrencyCode = "USD",
            Name = "美金"
        }, CancellationToken.None);
        
        // assert 
        expected.Should().Be(actual);
    }

    [Test]
    public async Task AddCurrencyCommandHandler_AddNonExistCurrency_ReturnTrue()
    {
        // arrange
        this._currencyRepositoryMock.Setup(item => item.AnyAsync(It.IsAny<Expression<Func<Currency, bool>>>()))
            .ReturnsAsync(false);
        var expected = true;
        
        // actual
        var actual = await _addCurrencyCommandHandler.Handle(new AddCurrencyCommand
        {
            CurrencyCode = "USD",
            Name = "美金"
        }, CancellationToken.None);

        // assert
        expected.Should().Be(actual);
        this._currencyRepositoryMock.Verify(item => item.AddAsync(It.IsAny<Currency>()), Times.Once);
        this._unitWorkMock.Verify(item => item.SaveChangesAsync(), Times.Once);
    }
}