using API.Admin.Domain.User.Events.Auth;
using MediatR;
using poc.core.api.net8.AppSettings;
using poc.core.api.net8.Enumerado;
using poc.core.api.net8.Helper;
using poc.core.api.net8.Interface;

namespace API.Admin.Feature.Auth.Events;

public class AuthResetEventHandler : INotificationHandler<AuthResetEvent>
{
    private readonly IMediator _mediator;
    private readonly IConfiguration _configuration;
    private readonly IAuthService _authService;

    public AuthResetEventHandler(IMediator mediator,
                                 IConfiguration configuration,
                                 IAuthService authService)
    {
        _mediator = mediator;
        _configuration = configuration;
        _authService = authService;
    }

    public async Task Handle(AuthResetEvent authevent, CancellationToken cancellationToken)
    {
        var token = _authService.GenerateJwtToken(authevent.Id.ToString(), authevent.Email);

        // Envio de aviso por email
        if (authevent.Notification == ENotificationType.Email)
            await EmailCommand(authevent, token);

        // Envio de aviso por SMS
        if (authevent.Notification == ENotificationType.SMS)
            await SMSCommand(authevent, token);

        // envio de aviso por WhatsApp
        if (authevent.Notification == ENotificationType.WhatsApp)
            await WhatsAppCommand(authevent, token);
    }

    #region Email, WhatsApp e SMS
    private async Task WhatsAppCommand(AuthResetEvent authevent, string token)
    {
        // ....
    }

    private async Task SMSCommand(AuthResetEvent authevent, string token)
    {
        // ....
    }

    private async Task EmailCommand(AuthResetEvent authevent, string token)
    {
        // ....
    }

    private string Email(AuthResetEvent entity, string token)
    {
        var passwordResetInfo = new PasswordResetInfo
        {
            UserName = entity.FirstName,
            ResetLink = $"{_configuration.GetValue<string>(AuthConsts.NEWPASSWORD)}{token}",
            ExpiryTime = TimeSpan.FromHours(2)
        };
        return EmailHelper.GeneratePasswordResetMessage(passwordResetInfo); ;
    }
    #endregion
}
