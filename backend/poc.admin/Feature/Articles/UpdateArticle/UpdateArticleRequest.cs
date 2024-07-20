namespace poc.admin.Feature.Articles.UpdateArticle;

public class UpdateArticleRequest
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
