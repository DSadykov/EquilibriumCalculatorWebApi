using NikCalcWebApi.Models.Requests;
using NikCalcWebApi.Models.Responses;

namespace NikCalcWebApi.Services.Authenticate;

public interface IUserService
{
    Task<AuthenticateResponse> Authenticate(AuthenticateRequest model);
    Task<AuthenticateResponse> Register(AuthenticateRequest userModel);
}
