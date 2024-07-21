using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using poc.admin.Infrastructure.Database;

namespace poc.admin.Feature.Articles.GetArticle;

public sealed class GetArticleHandler : IRequestHandler<GetArticleQuery, Result<List<ArticleResponse>>>
{
    private readonly EFSqlServerContext _dbContext;

    public GetArticleHandler(EFSqlServerContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<List<ArticleResponse>>> Handle(GetArticleQuery request, CancellationToken cancellationToken)
            => Result.Success(await _dbContext.Article.Select(a => new ArticleResponse
            {
                Id = a.Id,
                Title = a.Title,
                Description = a.Description,
                CreatedOnUtc = a.CreatedOnUtc,
                PublishedOnUtc = a.PublishedOnUtc
            }).ToListAsync());
}
