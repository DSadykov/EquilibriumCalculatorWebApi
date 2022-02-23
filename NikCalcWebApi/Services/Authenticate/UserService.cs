using NikCalcWebApi.Extensions;
using NikCalcWebApi.Models.Requests;
using NikCalcWebApi.Models.Responses;

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
        Models.UserModel? user = await _userRepository
            .GetUserByCredentialsAsync(model.UserName, _encryptionService.Encrypt(model.Password));
        if (user == null)
        {
            return new()
            {
                ErrorMessage = "Wrong email or password!"
            };
        }
        string? token = _configuration.GenerateJwtToken(user);
        return new() { Token = token };
    }

    public async Task<AuthenticateResponse> Register(AuthenticateRequest userModel)
    {
        if (await _userRepository.CheckEmailTakenAsync(userModel.UserName))
        {
            return new()
            {
                ErrorMessage = "Email is already taken!"
            };
        }
        string? encryptedPassword = _encryptionService.Encrypt(userModel.Password);
        Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<Models.UserModel>? user = await _userRepository.AddUserAsync(new()
        {
            UserName = userModel.UserName,
            Password = encryptedPassword,
        });
        Task? saveTask = _userRepository.SaveChangesAsync();
        string? token = _configuration.GenerateJwtToken(user.Entity);
        await saveTask;
        return new() { Token = token };
    }
}
