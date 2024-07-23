using MediatR;
using poc.admin.Infrastructure.Database.Repositories.Interfaces;
using poc.core.api.net8.Interface;
using poc.core.api.net8.Response;

namespace poc.admin.Feature.Users.GetArticle;

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, ApiResult<List<UserQueryModel>>>
{
    private readonly IUserRepository _repo;
    private readonly IRedisCacheService<List<UserQueryModel>> _cacheService;
    public GetUserQueryHandler(IUserRepository repo, IRedisCacheService<List<UserQueryModel>> cacheService)
    {
        _repo = repo;
        _cacheService = cacheService;
    }

    public async Task<ApiResult<List<UserQueryModel>>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        const string cacheKey = nameof(GetUserQuery);
        var users = await _cacheService.GetOrCreateAsync(cacheKey, () => _repo.GetAllAsync(), TimeSpan.FromHours(2));

        if (users is not null && users.Any())
            return ApiResult<List<UserQueryModel>>.CreateSuccess(users, "Usuários recuperados com sucesso.");

        return ApiResult<List<UserQueryModel>>.CreateSuccess(new List<UserQueryModel>(), "Nenhum usuário encontrado.");
    }
}
