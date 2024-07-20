using Ardalis.Result;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using poc.vertical.slices.net8.Database;

namespace poc.admin.Feature.Articles.DeleteArticle;

public sealed class DeleteArticleHandler : IRequestHandler<DeleteArticleCommand, Result>
{
    private readonly EFSqlServerContext _dbContext;
    private readonly IValidator<DeleteArticleCommand> _validator;

    public DeleteArticleHandler(EFSqlServerContext dbContext, IValidator<DeleteArticleCommand> validator)
    {
        _dbContext = dbContext;
        _validator = validator;
    }
    public async Task<Result> Handle(DeleteArticleCommand request, CancellationToken cancellationToken)
    {
        // Validate the command
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Result.Invalid(validationResult.Errors.Select(e => new ValidationError
            {
                ErrorMessage = e.ErrorMessage
            }).ToList());

        var article = await _dbContext.Article.Where(a => a.Id == request.Id).FirstOrDefaultAsync(cancellationToken);

        if (article is null)
            return Result.Error("Id não encontrado");

        _dbContext.Remove(article);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Result.SuccessWithMessage("Registro removido");
    }
}
