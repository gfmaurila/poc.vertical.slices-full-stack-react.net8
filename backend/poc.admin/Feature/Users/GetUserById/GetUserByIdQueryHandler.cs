using Ardalis.Result;
using MediatR;
using poc.admin.Feature.Users.GetArticle;
using poc.admin.Infrastructure.Database.Repositories.Interfaces;
using poc.core.api.net8.Interface;

namespace poc.admin.Feature.Users.GetUserById;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<UserQueryModel>>
{
    private readonly IUserRepository _repo;
    private readonly IRedisCacheService<UserQueryModel> _cacheService;
    private readonly GetUserByIdQueryValidator _validator;
    public GetUserByIdQueryHandler(IUserRepository repo,
                                   IRedisCacheService<UserQueryModel> cacheService,
                                   GetUserByIdQueryValidator validator)
    {
        _repo = repo;
        _cacheService = cacheService;
        _validator = validator;
    }

    public async Task<Result<UserQueryModel>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Result.Invalid(validationResult.Errors.Select(e => new ValidationError
            {
                ErrorMessage = e.ErrorMessage
            }).ToList());

        var cacheKey = $"{nameof(GetUserByIdQuery)}_{request.Id}";

        var modelRedis = await _cacheService.GetAsync(cacheKey);

        if (modelRedis is not null)
            return Result.Success(modelRedis);

        var entity = await _repo.GetByIdAsync(request.Id);

        if (entity is not null)
        {
            var model = await _cacheService.GetOrCreateAsync(cacheKey, () => _repo.GetByIdAsync(request.Id), TimeSpan.FromHours(2));
            return Result.Success(model);
        }

        return Result.NotFound($"Nenhum registro encontrado pelo Id: {request.Id}");
    }

}