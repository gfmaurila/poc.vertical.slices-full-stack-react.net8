﻿using Ardalis.Result;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace poc.admin.Feature.Users.UpdatePassword;

public class UpdatePasswordUserCommand : IRequest<Result>
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; }
}
