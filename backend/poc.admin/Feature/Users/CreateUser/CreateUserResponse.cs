using poc.core.api.net8;

namespace poc.admin.Feature.Users.CreateUser;

public class CreateUserResponse : BaseResponse
{
    public CreateUserResponse(Guid id) => Id = id;

    public Guid Id { get; }
}
