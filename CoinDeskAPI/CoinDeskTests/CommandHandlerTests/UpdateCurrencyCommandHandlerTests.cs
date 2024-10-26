using CoinDesk.Domain.CommandHandler;
using CoinDesk.Infrastructure.Model;
using CoinDesk.Infrastructure.Repository.Base;
using CoinDesk.Infrastructure.Repository.Interfaces;
using CoinDesk.Model.Command;
using FluentAssertions;
using Moq;

namespace CoinDeskTests.CommandHandlerTests;

public class UpdateCurrencyCommandHandlerTests
{
    private UpdateCurrencyCommandHandler _updateCurrencyCommandHandler;
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private Mock<ICurrencyRepository> _currencyRepositoryMock;

    [SetUp]
    public void SetUp()
    {
        _currencyRepositoryMock = new Mock<ICurrencyRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _unitOfWorkMock.Setup(item => item.CurrencyRepository).Returns(_currencyRepositoryMock.Object);
        _unitOfWorkMock.Setup(item => item.SaveChangesAsync()).ReturnsAsync(1);
        _updateCurrencyCommandHandler = new UpdateCurrencyCommandHandler(_unitOfWorkMock.Object);
    }

    [Test]
    public async Task UpdateCurrencyCommandHandler_UpdateNonExistCurrency_ReturnFalse()
    {
        // arrange
        _currencyRepositoryMock.Setup(item => item.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Currency)null);
        var expected = false;
        
        // actual
        var actual = await _updateCurrencyCommandHandler.Handle(new UpdateCurrencyCommand
        {
            Id = Guid.NewGuid(),
            Name = "美金"
        }, CancellationToken.None);
        
        // assert
        expected.Should().Be(actual);
    }
    
    [Test]
    public async Task UpdateCurrencyCommandHandler_UpdateExistCurrency_ReturnTrue()
    {
        // arrange
        var guid = Guid.NewGuid();
        var returnMockCurrency = new Currency
        {
            Id = guid,
            Name = "美金"
        };
        _currencyRepositoryMock.Setup(item => item.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(returnMockCurrency);
        var expected = true;
        
        // actual
        var actual = await _updateCurrencyCommandHandler.Handle(new UpdateCurrencyCommand
        {
            Id = guid,
            Name = "修改美金"
        }, CancellationToken.None);
        
        // assert
        _currencyRepositoryMock.Verify(item => item.UpdateAsync(It.IsAny<Currency>()), Times.Once);
        _unitOfWorkMock.Verify(item => item.SaveChangesAsync(), Times.Once);
        expected.Should().Be(actual);
        
        
    }
}