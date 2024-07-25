using MediatR;
using poc.core.api.net8.Response;
using System.ComponentModel.DataAnnotations;

namespace poc.admin.Feature.Users.UpdateEmail;

public class UpdateEmailUserCommand : IRequest<ApiResult<UpdateEmailUserResponse>>
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(200)]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
}
