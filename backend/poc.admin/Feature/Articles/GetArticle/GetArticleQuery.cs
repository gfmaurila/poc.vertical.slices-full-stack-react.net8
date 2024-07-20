using Ardalis.Result;
using MediatR;

namespace poc.admin.Feature.Articles.GetArticle;

public class GetArticleQuery : IRequest<Result<List<ArticleResponse>>>
{
}
