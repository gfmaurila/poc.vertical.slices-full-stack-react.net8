using Ardalis.Result;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using poc.vertical.slices.net8.Database;

namespace poc.admin.Feature.Articles.UpdateArticle;

public sealed class UpdateArticleHandler : IRequestHandler<UpdateArticleCommand, Result<ArticleIdResponse>>
{
    private readonly EFSqlServerContext _dbContext;
    private readonly IValidator<UpdateArticleCommand> _validator;

    public UpdateArticleHandler(EFSqlServerContext dbContext, IValidator<UpdateArticleCommand> validator)
    {
        _dbContext = dbContext;
        _validator = validator;
    }
    public async Task<Result<ArticleIdResponse>> Handle(UpdateArticleCommand request, CancellationToken cancellationToken)
    {
        // Validate the command
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Result.Invalid(validationResult.Errors.Select(e => new ValidationError
            {
                ErrorMessage = e.ErrorMessage
            }).ToList());

        var article = await _dbContext.Article
                                       .Where(a => a.Id == request.Id)
                                       .FirstOrDefaultAsync(cancellationToken);

        if (article is null)
            return Result.Error("Id não encontrado");

        article.Title = request.Title;
        article.Description = request.Description;

        _dbContext.Entry(article).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Result.Success(new ArticleIdResponse(article.Id), "Registro alterado");
    }
}