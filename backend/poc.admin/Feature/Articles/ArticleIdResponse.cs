using poc.core.api.net8;

namespace poc.admin.Feature.Articles;

public class ArticleIdResponse : BaseResponse
{
    public ArticleIdResponse(Guid id) => Id = id;

    public Guid Id { get; }
}
