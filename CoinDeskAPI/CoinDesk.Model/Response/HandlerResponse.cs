using CoinDesk.Model.Enum;

namespace CoinDesk.Model.Response;

public class HandlerResponse
{
    public ApiResponseStatus Status { get; set; }
    
    public object Data { get; set; }
}