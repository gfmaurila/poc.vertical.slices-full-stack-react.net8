using Ardalis.Result;
using MediatR;
using poc.admin.Infrastructure.Database.Repositories.Interfaces;
using poc.core.api.net8.Interface;

namespace poc.admin.Feature.Users.GetArticle;

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, Result<List<UserQueryModel>>>
{
    private readonly IUserRepository _repo;
    private readonly IRedisCacheService<List<UserQueryModel>> _cacheService;
    public GetUserQueryHandler(IUserRepository repo, IRedisCacheService<List<UserQueryModel>> cacheService)
    {
        _repo = repo;
        _cacheService = cacheService;
    }

    public async Task<Result<List<UserQueryModel>>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        const string cacheKey = nameof(GetUserQuery);
        return Result.Success(await _cacheService.GetOrCreateAsync(cacheKey, _repo.GetAllAsync, TimeSpan.FromHours(2)));
    }
}
