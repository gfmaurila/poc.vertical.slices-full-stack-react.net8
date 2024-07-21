using Ardalis.Result;
using MediatR;
using poc.admin.Infrastructure.Database.Repositories.Interfaces;
using poc.core.api.net8.Extensions;

namespace poc.admin.Feature.Users.UpdatePassword;

public class UpdatePasswordUserCommandHandler : IRequestHandler<UpdatePasswordUserCommand, Result>
{
    private readonly UpdatePasswordUserCommandValidator _validator;
    private readonly IUserRepository _repo;
    private readonly ILogger<UpdatePasswordUserCommandHandler> _logger;
    private readonly IMediator _mediator;
    public UpdatePasswordUserCommandHandler(ILogger<UpdatePasswordUserCommandHandler> logger,
                                    IUserRepository repo,
                                    UpdatePasswordUserCommandValidator validator,
                                    IMediator mediator)
    {
        _repo = repo;
        _logger = logger;
        _validator = validator;
        _mediator = mediator;
    }
    public async Task<Result> Handle(UpdatePasswordUserCommand request, CancellationToken cancellationToken)
    {
        // Validanto a requisição.
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return Result.Invalid(validationResult.Errors.Select(e => new ValidationError
            {
                ErrorMessage = e.ErrorMessage
            }).ToList());

        // Obtendo o registro da base.
        var entity = await _repo.Get(request.Id);
        if (entity == null)
            return Result.NotFound($"Nenhum registro encontrado pelo Id: {request.Id}");

        entity.ChangePassword(Password.ComputeSha256Hash(request.Password));

        await _repo.Update(entity);

        // Executa eventos
        foreach (var domainEvent in entity.DomainEvents)
            await _mediator.Publish(domainEvent);

        entity.ClearDomainEvents();

        return Result.SuccessWithMessage("Atualizado com sucesso!");
    }
}
