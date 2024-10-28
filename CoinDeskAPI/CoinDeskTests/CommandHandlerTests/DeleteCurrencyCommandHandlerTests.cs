using System.Linq.Expressions;
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

public class DeleteCurrencyCommandHandlerTests
{
    private DeleteCurrencyCommandHandler _deleteCurrencyCommandHandler;
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private Mock<ICurrencyRepository> _currencyRepositoryMock;

    [SetUp]
    public void SetUp()
    {
        _currencyRepositoryMock = new Mock<ICurrencyRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _unitOfWorkMock.Setup(item => item.CurrencyRepository).Returns(_currencyRepositoryMock.Object);
        _unitOfWorkMock.Setup(item => item.SaveChangesAsync()).ReturnsAsync(1);
        _deleteCurrencyCommandHandler = new DeleteCurrencyCommandHandler(_unitOfWorkMock.Object);
    }
    
    [Test]
    public async Task DeleteCurrencyCommandHandler_DeleteExistCurrency_ReturnSuccess()
    {
        // arrange
        _currencyRepositoryMock.Setup(item => item.AnyAsync(It.IsAny<Expression<Func<Currency, bool>>>()))
            .ReturnsAsync(true);
        var expected = new HandlerResponse
        {
            Status = ApiResponseStatus.Success
        };
        
        // actual
        var actual = await _deleteCurrencyCommandHandler.Handle(new DeleteCurrencyCommand
        {
            Id = Guid.NewGuid()
        }, CancellationToken.None);
        
        // assert
        expected.Should().BeEquivalentTo(actual);
        _currencyRepositoryMock.Verify(item => item.DeleteAsync(It.IsAny<Guid>()), Times.Once);
        _unitOfWorkMock.Verify(item => item.SaveChangesAsync(), Times.Once);
    }
}