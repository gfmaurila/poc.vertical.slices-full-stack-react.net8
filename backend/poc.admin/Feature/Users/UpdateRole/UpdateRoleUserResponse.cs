using poc.core.api.net8;

namespace poc.admin.Feature.Users.UpdateRole;

public class UpdateRoleUserResponse : BaseResponse
{
    public UpdateRoleUserResponse(Guid id) => Id = id;
    public Guid Id { get; }
}
