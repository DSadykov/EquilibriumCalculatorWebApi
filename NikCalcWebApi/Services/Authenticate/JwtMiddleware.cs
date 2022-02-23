using Microsoft.IdentityModel.Tokens;
using NikCalcWebApi.Services.Authenticate;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ImageConverterWebApi.Services;

public class JwtMiddleware : IMiddleware
{
    private readonly IUserService _userService;
    private readonly IConfiguration _configuration;

    public JwtMiddleware(IUserService userService, IConfiguration configuration)
    {
        _userService = userService;
        _configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        string? token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token != null)
        {
            AttachUserToContext(context, token);
        }

        await next(context);
    }

    private void AttachUserToContext(HttpContext context, string token)
    {

        JwtSecurityTokenHandler? tokenHandler = new JwtSecurityTokenHandler();
        // min 16 characters
        byte[]? key = Encoding.ASCII.GetBytes(_configuration["Secret"]);
        tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        }, out SecurityToken validatedToken);

        JwtSecurityToken? jwtToken = (JwtSecurityToken)validatedToken;
        int userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);
        context.Items["UserId"] = userId;
    }
}
