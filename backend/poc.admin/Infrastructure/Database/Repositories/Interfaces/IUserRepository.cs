using poc.admin.Domain.User;
using poc.admin.Feature.Users.GetArticle;
using poc.core.api.net8.Abstractions;
using poc.core.api.net8.ValueObjects;

namespace poc.admin.Infrastructure.Database.Repositories.Interfaces;

public interface IUserRepository : IBaseRepository<UserEntity>
{
    Task<bool> ExistsByEmailAsync(Email email);
    Task<bool> ExistsByEmailAsync(Email email, Guid currentId);
    Task<List<UserQueryModel>> GetAllAsync();
}

