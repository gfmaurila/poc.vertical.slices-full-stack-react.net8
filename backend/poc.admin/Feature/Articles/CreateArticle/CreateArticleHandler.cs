using Ardalis.Result;
using FluentValidation;
using MediatR;
using poc.vertical.slices.net8.Database;
using poc.vertical.slices.net8.Domain;

namespace poc.admin.Feature.Articles.CreateArticle;

public sealed class CreateArticleHandler : IRequestHandler<CreateArticleCommand, Result<ArticleIdResponse>>
{
    private readonly EFSqlServerContext _dbContext;
    private readonly IValidator<CreateArticleCommand> _validator;

    public CreateArticleHandler(EFSqlServerContext dbContext, IValidator<CreateArticleCommand> validator)
    {
        _dbContext = dbContext;
        _validator = validator;
    }
    public async Task<Result<ArticleIdResponse>> Handle(CreateArticleCommand request, CancellationToken cancellationToken)
    {
        // Validate the command
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Result.Invalid(validationResult.Errors.Select(e => new ValidationError
            {
                ErrorMessage = e.ErrorMessage
            }).ToList());

        var article = new Article
        {
            Title = request.Title,
            Description = request.Description,
            CreatedOnUtc = DateTime.UtcNow,
        };

        _dbContext.Add(article);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return Result.Success(new ArticleIdResponse(article.Id), "Registro cadastrado");
    }
}
