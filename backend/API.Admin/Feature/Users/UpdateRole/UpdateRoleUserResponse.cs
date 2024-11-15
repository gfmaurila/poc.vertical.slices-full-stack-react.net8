using Common.Net8;

namespace API.Admin.Feature.Users.UpdateRole;

public class UpdateRoleUserResponse : BaseResponse
{
    public UpdateRoleUserResponse(Guid id) => Id = id;
    public Guid Id { get; }
}
