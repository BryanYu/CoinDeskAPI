using CoinDesk.Infrastructure.Repository.Base;
using CoinDesk.Model.Command;
using CoinDesk.Model.Enum;
using CoinDesk.Model.Response;
using MediatR;

namespace CoinDesk.Domain.CommandHandler;

public class UpdateCurrencyCommandHandler : IRequestHandler<UpdateCurrencyCommand, HandlerResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCurrencyCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<HandlerResponse> Handle(UpdateCurrencyCommand request, CancellationToken cancellationToken)
    {
        var existCurrency = await _unitOfWork.CurrencyRepository.GetByIdAsync(request.Id);
        if (existCurrency == null)
        {
            return new HandlerResponse
            {
                Status = ApiResponseStatus.CurrencyNotExist
            };
        }
        existCurrency.Name = request.Name;
        existCurrency.UpdateTime = DateTime.Now;

        await _unitOfWork.CurrencyRepository.UpdateAsync(existCurrency);
        var result = await _unitOfWork.SaveChangesAsync();
        return new HandlerResponse
        {
            Status = ApiResponseStatus.Success
        };
    }
}