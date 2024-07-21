using Ardalis.Result;
using MediatR;
using poc.admin.Database.Repositories.Interfaces;
using poc.admin.Domain.User;
using poc.core.api.net8.Extensions;
using poc.core.api.net8.ValueObjects;

namespace poc.admin.Feature.Users.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<CreateUserResponse>>
{
    private readonly CreateUserCommandValidator _validator;
    private readonly IUserRepository _repo;
    private readonly ILogger<CreateUserCommandHandler> _logger;
    private readonly IMediator _mediator;
    public CreateUserCommandHandler(ILogger<CreateUserCommandHandler> logger,
                                    IUserRepository repo,
                                    IMediator mediator,
                                    CreateUserCommandValidator validator)
    {
        _repo = repo;
        _logger = logger;
        _validator = validator;
        _mediator = mediator;
    }
    public async Task<Result<CreateUserResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Result.Invalid(validationResult.Errors.Select(e => new ValidationError
            {
                ErrorMessage = e.ErrorMessage
            }).ToList());

        var email = new Email(request.Email);
        var phone = new PhoneNumber(request.Phone);

        if (await _repo.ExistsByEmailAsync(email))
            return Result.Error("O endereço de e-mail informado já está sendo utilizado.");

        var entity = new UserEntity(request.FirstName,
            request.LastName,
            request.Gender,
            request.Notification,
            email,
            phone,
            Password.ComputeSha256Hash(request.Password),
            request.RoleUserAuth,
            request.DateOfBirth);

        await _repo.Create(entity);

        //await _unitOfWork.SaveChangesAsync();

        // Executa eventos
        foreach (var domainEvent in entity.DomainEvents)
            await _mediator.Publish(domainEvent);

        entity.ClearDomainEvents();

        return Result.Success(new CreateUserResponse(entity.Id), "Cadastrado com sucesso!");
    }
}
