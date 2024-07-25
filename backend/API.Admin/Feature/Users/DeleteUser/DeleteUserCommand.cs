using MediatR;
using poc.core.api.net8.Response;

namespace API.Admin.Feature.Users.DeleteUser;

public class DeleteUserCommand : IRequest<ApiResult<DeleteUserResponse>>
{
    public DeleteUserCommand(Guid id) => Id = id;

    public Guid Id { get; private set; }
}
