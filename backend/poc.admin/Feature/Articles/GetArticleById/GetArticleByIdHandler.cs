using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using poc.admin.Infrastructure.Database;

namespace poc.admin.Feature.Articles.GetArticleById;

internal sealed class GetArticleByIdHandler : IRequestHandler<GetArticleByIdQuery, Result<ArticleResponse>>
{
    private readonly EFSqlServerContext _dbContext;

    public GetArticleByIdHandler(EFSqlServerContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<ArticleResponse>> Handle(GetArticleByIdQuery request, CancellationToken cancellationToken)
    {
        var article = await _dbContext.Article
                                       .Where(a => a.Id == request.Id)
                                       .Select(a => new ArticleResponse
                                       {
                                           Id = a.Id,
                                           Title = a.Title,
                                           Description = a.Description,
                                           CreatedOnUtc = a.CreatedOnUtc,
                                           PublishedOnUtc = a.PublishedOnUtc
                                       })
                                       .FirstOrDefaultAsync(cancellationToken);

        if (article is null)
            return Result.Error("Id não encontrado");

        return article;
    }
}
