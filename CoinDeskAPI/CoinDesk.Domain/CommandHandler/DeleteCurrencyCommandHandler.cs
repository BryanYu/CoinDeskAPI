﻿using CoinDesk.Infrastructure.Repository.Base;
using CoinDesk.Model.Command;
using CoinDesk.Model.Enum;
using CoinDesk.Model.Response;
using MediatR;

namespace CoinDesk.Domain.CommandHandler;

public class DeleteCurrencyCommandHandler : IRequestHandler<DeleteCurrencyCommand, HandlerResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCurrencyCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<HandlerResponse> Handle(DeleteCurrencyCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.CurrencyRepository.DeleteAsync(request.Id);
        var result = await _unitOfWork.SaveChangesAsync();
        return new HandlerResponse
        {
            Status = ApiResponseStatus.Success
        };
    }
}