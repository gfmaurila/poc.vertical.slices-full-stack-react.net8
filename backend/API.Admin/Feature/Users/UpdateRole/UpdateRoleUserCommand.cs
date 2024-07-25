using MediatR;
using poc.core.api.net8.Response;
using System.ComponentModel.DataAnnotations;

namespace API.Admin.Feature.Users.UpdateRole;

public class UpdateRoleUserCommand : IRequest<ApiResult<UpdateRoleUserResponse>>
{
    [Required]
    public Guid Id { get; set; }

    public List<string> RoleUserAuth { get; set; } = new List<string>();
}
