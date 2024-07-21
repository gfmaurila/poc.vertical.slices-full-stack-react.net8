using Ardalis.Result;
using MediatR;
using poc.admin.Infrastructure.Database.Repositories.Interfaces;

namespace poc.admin.Feature.Users.DeleteUser;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result>
{
    private readonly DeleteUserCommandValidator _validator;
    private readonly IUserRepository _repo;
    private readonly ILogger<DeleteUserCommandHandler> _logger;
    private readonly IMediator _mediator;
    public DeleteUserCommandHandler(ILogger<DeleteUserCommandHandler> logger,
                                    IUserRepository repo,
                                    DeleteUserCommandValidator validator,
                                    IMediator mediator)
    {
        _repo = repo;
        _logger = logger;
        _validator = validator;
        _mediator = mediator;
    }

    public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
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

        entity.Delete();

        await _repo.Remove(entity);

        // Executa eventos
        foreach (var domainEvent in entity.DomainEvents)
            await _mediator.Publish(domainEvent);

        entity.ClearDomainEvents();

        return Result.SuccessWithMessage("Removido com sucesso!");
    }
}
