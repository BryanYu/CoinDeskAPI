using CoinDesk.Infrastructure.Model;
using CoinDesk.Infrastructure.Repository.Base;
using CoinDesk.Model.Command;
using MediatR;

namespace CoinDesk.Domain.CommandHandler;

public class AddCurrencyCommandHandler : IRequestHandler<AddCurrencyCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddCurrencyCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(AddCurrencyCommand request, CancellationToken cancellationToken)
    {
        var exist = await _unitOfWork.CurrencyRepository.AnyAsync(item => item.CurrencyCode == request.CurrencyCode);
        if (exist)
        {
            return false;
        }

        var newCurrency = new Currency
        {
            CurrencyCode = request.CurrencyCode.ToUpper(),
            Name = request.Name,
            CreatedTime = DateTime.Now
        };
        await _unitOfWork.CurrencyRepository.AddAsync(newCurrency);
        var result = await _unitOfWork.SaveChangesAsync();
        return result > 0;
    }
}