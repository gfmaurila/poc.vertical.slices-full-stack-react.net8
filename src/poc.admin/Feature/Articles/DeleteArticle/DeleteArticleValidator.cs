using FluentValidation;

namespace poc.admin.Feature.Articles.DeleteArticle;

public class DeleteArticleValidator : AbstractValidator<DeleteArticleCommand>
{
    public DeleteArticleValidator()
    {
        RuleFor(c => c.Id).NotEmpty();
    }
}
