using CoinDesk.Model.Command;
using CoinDesk.Model.Enum;
using CoinDesk.Model.Response;
using CoinDesk.Utility;
using MediatR;

namespace CoinDesk.Domain.CommandHandler;

public class GenerateTokenCommandHandler : IRequestHandler<GenerateTokenCommand, HandlerResponse>
{
    private readonly List<(string account, string password)> _users = new List<(string account, string password)>
    {
        new ValueTuple<string, string>("admin", "$2a$11$LrL6iEfjszmodGuqdW7FsuAGV1uw.1Ax2GrxXvXiT6.vPvW2ZLruW" ) // for example used, need store database on production
    };
    
    private readonly JwtTokenGenerator _jwtTokenGenerator;

    public GenerateTokenCommandHandler(JwtTokenGenerator jwtTokenGenerator)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
    }
    public async Task<HandlerResponse> Handle(GenerateTokenCommand request, CancellationToken cancellationToken)
    {
        if (_users.All(item => item.account != request.Account))
        {
            return new HandlerResponse()
            {
                Status = ApiResponseStatus.UserNotExist
            };
        }
        var user = _users.FirstOrDefault(item => item.account == request.Account);

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.password))
        {
            return new HandlerResponse()
            {
                Status = ApiResponseStatus.UserPasswordError
            };
        }

        return new HandlerResponse
        {
            Data = _jwtTokenGenerator.GenerateJwtToken(request.Account),
            Status = ApiResponseStatus.Success
        };
    }
}