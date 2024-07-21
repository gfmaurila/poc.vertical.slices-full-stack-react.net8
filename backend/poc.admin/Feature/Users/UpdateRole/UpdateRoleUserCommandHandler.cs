using Ardalis.Result;
using MediatR;
using poc.admin.Infrastructure.Database.Repositories.Interfaces;

namespace poc.admin.Feature.Users.UpdateRole;

public class UpdateRoleUserCommandHandler : IRequestHandler<UpdateRoleUserCommand, Result>
{
    private readonly UpdateRoleUserCommandValidator _validator;
    private readonly IUserRepository _repo;
    private readonly ILogger<UpdateRoleUserCommandHandler> _logger;
    private readonly IMediator _mediator;
    public UpdateRoleUserCommandHandler(ILogger<UpdateRoleUserCommandHandler> logger,
                                    IUserRepository repo,
                                    UpdateRoleUserCommandValidator validator,
                                    IMediator mediator)
    {
        _repo = repo;
        _logger = logger;
        _validator = validator;
        _mediator = mediator;
    }
    public async Task<Result> Handle(UpdateRoleUserCommand request, CancellationToken cancellationToken)
    {
        // Obtendo o registro da base.
        var entity = await _repo.Get(request.Id);
        if (entity == null)
            return Result.NotFound($"Nenhum registro encontrado pelo Id: {request.Id}");

        entity.UpdateRole(request.RoleUserAuth);

        await _repo.Update(entity);

        // Executa eventos
        foreach (var domainEvent in entity.DomainEvents)
            await _mediator.Publish(domainEvent);

        entity.ClearDomainEvents();

        return Result.SuccessWithMessage("Atualizado com sucesso!");
    }
}
