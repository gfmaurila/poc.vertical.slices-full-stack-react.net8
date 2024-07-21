﻿using Ardalis.Result;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace poc.admin.Feature.Users.UpdateEmail;

public class UpdateEmailUserCommand : IRequest<Result>
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(200)]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
}
