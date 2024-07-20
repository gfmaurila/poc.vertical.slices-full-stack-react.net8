using FluentValidation;

namespace poc.admin.Feature.Articles.CreateArticle;

public class CreateArticleValidator : AbstractValidator<CreateArticleCommand>
{
    public CreateArticleValidator()
    {
        RuleFor(c => c.Title).NotEmpty();
        RuleFor(c => c.Description).NotEmpty();
    }
}
