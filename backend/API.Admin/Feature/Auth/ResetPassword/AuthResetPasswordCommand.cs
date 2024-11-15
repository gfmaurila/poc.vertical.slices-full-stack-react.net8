using Common.Net8.Response;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace API.Admin.Feature.Auth.ResetPassword;

public class AuthResetPasswordCommand : IRequest<ApiResult<AuthResetPasswordResponse>>
{
    [Required]
    [MaxLength(200)]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
}