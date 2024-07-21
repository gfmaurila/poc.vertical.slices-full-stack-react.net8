using Ardalis.Result;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace poc.admin.Feature.Users.UpdateRole;

public class UpdateRoleUserCommand : IRequest<Result>
{
    [Required]
    public Guid Id { get; set; }

    public List<string> RoleUserAuth { get; set; } = new List<string>();
}
