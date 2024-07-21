using Ardalis.Result;
using MediatR;

namespace poc.admin.Feature.Users.DeleteUser;

public class DeleteUserCommand : IRequest<Result>
{
    public DeleteUserCommand(Guid id) => Id = id;

    public Guid Id { get; private set; }
}
