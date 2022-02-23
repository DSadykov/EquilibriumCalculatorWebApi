using NikCalcWebApi.Requests;
using NikCalcWebApi.Responses;
using NikCalcWebApi.Extensions;

namespace NikCalcWebApi.Services.Authenticate;

public class UserService : IUserService
{
    private readonly DbRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly EncryptionService _encryptionService;

    public UserService(DbRepository userRepository, IConfiguration configuration, EncryptionService encryptionService)
    {
        _userRepository = userRepository;
        _configuration = configuration;
        _encryptionService = encryptionService;
    }

    public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model)
    {
        var user = await _userRepository
            .GetUserByCredentials(model.UserName, _encryptionService.Encrypt( model.Password));

        if (user == null)
        {
            return new()
            {
                ErrorMessage = "Wrong email or password!"
            };
        }

        var token = _configuration.GenerateJwtToken(user);

        return new() { Token = token };
    }

    public async Task<AuthenticateResponse> Register(AuthenticateRequest userModel)
    {
        if (await _userRepository.CheckEmailTaken(userModel.UserName))
        {
            return new()
            {
                ErrorMessage = "Email is already taken!"
            };
        }
        var encryptedPassword = _encryptionService.Encrypt(userModel.Password);
        await _userRepository.AddUser(new()
        {
            UserName = userModel.UserName,
            Password = encryptedPassword,
        });
        var response = Authenticate(new AuthenticateRequest
        {
            UserName = userModel.UserName,
            Password = userModel.Password
        });
        return await response;
    }
}
