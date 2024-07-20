using Ardalis.Result;
using MediatR;

namespace poc.admin.Feature.Articles.GetArticleById;

public class GetArticleByIdQuery : IRequest<Result<ArticleResponse>>
{
    public Guid Id { get; set; }
}

