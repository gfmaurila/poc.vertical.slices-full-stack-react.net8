using Ardalis.Result;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace API.Admin.Feature.Auth;

public class AuthCommand : IRequest<Result<AuthTokenResponse>>
{
    [Required]
    [MaxLength(200)]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}
