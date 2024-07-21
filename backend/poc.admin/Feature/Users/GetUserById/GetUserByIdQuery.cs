using Ardalis.Result;
using MediatR;
using poc.admin.Feature.Users.GetArticle;

namespace poc.admin.Feature.Users.GetUserById;


public class GetUserByIdQuery : IRequest<Result<UserQueryModel>>
{
    public GetUserByIdQuery(Guid id)
    {
        Id = id;
    }
    public Guid Id { get; private set; }
}