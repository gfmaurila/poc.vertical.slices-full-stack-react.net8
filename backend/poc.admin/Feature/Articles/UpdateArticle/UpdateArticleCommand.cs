using Ardalis.Result;
using MediatR;

namespace poc.admin.Feature.Articles.UpdateArticle;

public class UpdateArticleCommand : IRequest<Result<ArticleIdResponse>>
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = new();
}
