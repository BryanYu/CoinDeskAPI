using CoinDesk.Infrastructure.Model;
using CoinDesk.Infrastructure.Repository.Base;
using CoinDesk.Model.Command;
using CoinDesk.Model.Enum;
using CoinDesk.Model.Response;
using MediatR;

namespace CoinDesk.Domain.CommandHandler;

public class AddCurrencyCommandHandler : IRequestHandler<AddCurrencyCommand, HandlerResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddCurrencyCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<HandlerResponse> Handle(AddCurrencyCommand request, CancellationToken cancellationToken)
    {
        var exist = await _unitOfWork.CurrencyRepository.AnyAsync(item => item.CurrencyCode == request.CurrencyCode);
        if (exist)
        {
            return new HandlerResponse
            {
                Status = ApiResponseStatus.CurrencyExist
            };
        }
        var newCurrency = new Currency
        {
            CurrencyCode = request.CurrencyCode.ToUpper(),
            Name = request.Name,
            CreatedTime = DateTime.Now
        };
        await _unitOfWork.CurrencyRepository.AddAsync(newCurrency);
        var result = await _unitOfWork.SaveChangesAsync();
        return new HandlerResponse
        {
            Status = ApiResponseStatus.Success
        };
    }
}