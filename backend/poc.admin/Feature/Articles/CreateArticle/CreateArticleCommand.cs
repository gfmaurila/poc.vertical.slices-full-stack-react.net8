using Ardalis.Result;
using MediatR;

namespace poc.admin.Feature.Articles.CreateArticle;

public class CreateArticleCommand : IRequest<Result<ArticleIdResponse>>
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = new();
}