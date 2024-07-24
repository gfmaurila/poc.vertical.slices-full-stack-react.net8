using MediatR;
using poc.admin.Feature.Users.GetArticle;
using poc.core.api.net8.Response;

namespace poc.admin.Feature.Users.GetUserById;


public class GetUserByIdQuery : IRequest<ApiResult<UserQueryModel>>
{
    public GetUserByIdQuery(Guid id)
    {
        Id = id;
    }
    public Guid Id { get; private set; }
}