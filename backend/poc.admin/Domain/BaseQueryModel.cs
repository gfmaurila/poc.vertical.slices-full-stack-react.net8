using poc.core.api.net8.Abstractions;

namespace poc.admin.Domain;

public abstract class BaseQueryModel : IQueryModel<Guid>
{
    public Guid Id { get; set; } = Guid.NewGuid();
}
