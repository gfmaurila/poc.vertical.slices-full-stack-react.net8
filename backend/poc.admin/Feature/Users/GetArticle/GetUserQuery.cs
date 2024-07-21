using Ardalis.Result;
using MediatR;

namespace poc.admin.Feature.Users.GetArticle;

public class GetUserQuery : IRequest<Result<List<UserQueryModel>>>
{
    public GetUserQuery()
    {
    }
}
