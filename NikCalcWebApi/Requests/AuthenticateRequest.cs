using System.ComponentModel.DataAnnotations;

namespace NikCalcWebApi.Requests;

public class AuthenticateRequest
{
    [Required]
    public string UserName { get; set; }

    [Required]
    public string Password { get; set; }
}
