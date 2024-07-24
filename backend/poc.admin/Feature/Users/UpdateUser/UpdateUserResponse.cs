using poc.core.api.net8;

namespace poc.admin.Feature.Users.UpdateUser;

public class UpdateUserResponse : BaseResponse
{
    public UpdateUserResponse(Guid id) => Id = id;
    public Guid Id { get; }
}
