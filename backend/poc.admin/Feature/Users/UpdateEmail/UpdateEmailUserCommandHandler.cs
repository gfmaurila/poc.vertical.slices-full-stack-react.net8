using Ardalis.Result;
using MediatR;
using poc.admin.Infrastructure.Database.Repositories.Interfaces;
using poc.core.api.net8.ValueObjects;

namespace poc.admin.Feature.Users.UpdateEmail;

public class UpdateEmailUserCommandHandler : IRequestHandler<UpdateEmailUserCommand, Result>
{
    private readonly UpdateEmailUserCommandValidator _validator;
    private readonly IUserRepository _repo;
    private readonly ILogger<UpdateEmailUserCommandHandler> _logger;
    private readonly IMediator _mediator;
    public UpdateEmailUserCommandHandler(ILogger<UpdateEmailUserCommandHandler> logger,
                                    IUserRepository repo,
                                    UpdateEmailUserCommandValidator validator,
                                    IMediator mediator)
    {
        _repo = repo;
        _logger = logger;
        _validator = validator;
        _mediator = mediator;
    }

    public async Task<Result> Handle(UpdateEmailUserCommand request, CancellationToken cancellationToken)
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

        // Instanciando o VO Email.
        var newEmail = new Email(request.Email);

        // Verificiando se já existe um cliente com o endereço de e-mail.
        if (await _repo.ExistsByEmailAsync(newEmail, entity.Id))
            return Result.Error("O endereço de e-mail informado já está sendo utilizado.");

        entity.ChangeEmail(newEmail);

        await _repo.Update(entity);

        // Executa eventos
        foreach (var domainEvent in entity.DomainEvents)
            await _mediator.Publish(domainEvent);

        entity.ClearDomainEvents();

        return Result.SuccessWithMessage("Atualizado com sucesso!");
    }
}
