using MediatR;
using poc.core.api.net8.Response;

namespace poc.admin.Feature.Users.GetArticle;

public class GetUserQuery : IRequest<ApiResult<List<UserQueryModel>>>
{
    public GetUserQuery()
    {
    }
}
