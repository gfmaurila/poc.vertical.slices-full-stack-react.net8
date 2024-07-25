﻿using MediatR;
using poc.core.api.net8.Enumerado;
using poc.core.api.net8.Response;
using System.ComponentModel.DataAnnotations;

namespace API.Admin.Feature.Users.UpdateUser;

public class UpdateUserCommand : IRequest<ApiResult<UpdateUserResponse>>
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(100)]
    [DataType(DataType.Text)]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(100)]
    [DataType(DataType.Text)]
    public string LastName { get; set; }

    [Required]
    public EGender Gender { get; set; }

    [Required]
    public ENotificationType Notification { get; set; }

    [Required]
    [MaxLength(20)]
    [DataType(DataType.PhoneNumber)]
    public string Phone { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }
}
