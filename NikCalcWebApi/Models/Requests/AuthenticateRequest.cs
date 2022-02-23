using System.ComponentModel.DataAnnotations;

namespace NikCalcWebApi.Models.Requests;
public class AuthenticateRequest
{
    [Required]
    public string UserName { get; set; }

    [Required]
    public string Password { get; set; }
}
