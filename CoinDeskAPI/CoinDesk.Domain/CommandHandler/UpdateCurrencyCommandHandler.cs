using CoinDesk.Infrastructure.Repository.Base;
using CoinDesk.Model.Command;
using MediatR;

namespace CoinDesk.Domain.CommandHandler;

public class UpdateCurrencyCommandHandler : IRequestHandler<UpdateCurrencyCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCurrencyCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<bool> Handle(UpdateCurrencyCommand request, CancellationToken cancellationToken)
    {
        var existCurrency = await _unitOfWork.CurrencyRepository.GetByIdAsync(request.Id);
        if (existCurrency == null)
        {
            return false;
        }
        existCurrency.Name = request.Name;
        existCurrency.UpdateTime = DateTime.Now;

        await _unitOfWork.CurrencyRepository.UpdateAsync(existCurrency);
        var result = await _unitOfWork.SaveChangesAsync();
        return result > 0;

    }
}