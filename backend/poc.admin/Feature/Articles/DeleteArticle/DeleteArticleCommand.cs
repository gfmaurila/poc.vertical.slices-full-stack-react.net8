using Ardalis.Result;
using MediatR;

namespace poc.admin.Feature.Articles.DeleteArticle;

public class DeleteArticleCommand : IRequest<Result>
{
    public Guid Id { get; set; }
}
