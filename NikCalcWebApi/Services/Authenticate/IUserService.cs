using NikCalcWebApi.Requests;
using NikCalcWebApi.Responses;

namespace NikCalcWebApi.Services.Authenticate;

public interface IUserService
{
    Task<AuthenticateResponse> Authenticate(AuthenticateRequest model);
    Task<AuthenticateResponse> Register(AuthenticateRequest userModel);
}
