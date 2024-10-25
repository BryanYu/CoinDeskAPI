using CoinDesk.Infrastructure.Repository.Base;
using CoinDesk.Model.Command;
using MediatR;

namespace CoinDesk.Domain.CommandHandler;

public class DeleteCurrencyCommandHandler : IRequestHandler<DeleteCurrencyCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCurrencyCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteCurrencyCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.CurrencyRepository.DeleteAsync(request.Id);
        var result = await _unitOfWork.SaveChangesAsync();
        return result > 0;
    }
}