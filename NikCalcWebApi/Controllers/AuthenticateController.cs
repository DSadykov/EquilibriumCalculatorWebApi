using Microsoft.AspNetCore.Mvc;
using NikCalcWebApi.Requests;
using NikCalcWebApi.Responses;
using NikCalcWebApi.Services.Authenticate;

namespace NikCalcWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticateController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthenticateController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("authenticate")]
    public async Task<IActionResult> Authenticate(AuthenticateRequest model)
    {
        var response = _userService.Authenticate(model);
        return CheckResponse(await response);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(AuthenticateRequest userModel)
    {
        AuthenticateResponse? response = await _userService.Register(userModel);
        return CheckResponse(response);
    }

    private IActionResult CheckResponse(AuthenticateResponse response)
    {
        return response.Token is null ? BadRequest(new { message = response.ErrorMessage }) : Ok(response);
    }
}
