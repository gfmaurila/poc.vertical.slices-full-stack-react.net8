﻿using Common.Net8.Response;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace API.Admin.Feature.Auth.AuthNewPassword;

public class AuthNewPasswordCommand : IRequest<ApiResult<AuthNewPasswordResponse>>
{
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; }

    [Required]
    public string Token { get; set; }
}
