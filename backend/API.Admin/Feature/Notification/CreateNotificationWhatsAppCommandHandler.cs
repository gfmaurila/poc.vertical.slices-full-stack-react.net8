using API.Admin.Feature.Notification.Events;
using API.Admin.Feature.Notification.Producer;
using Ardalis.Result;
using MediatR;

namespace API.Admin.Feature.Notification;

public class CreateNotificationWhatsAppCommandHandler : IRequestHandler<CreateNotificationWhatsAppCommand, Result>
{

    private readonly ILogger<CreateNotificationWhatsAppCommandHandler> _logger;
    private readonly ITwilioProducer _twilioProducer;
    public CreateNotificationWhatsAppCommandHandler(ILogger<CreateNotificationWhatsAppCommandHandler> logger,
                                                    ITwilioProducer twilioProducer)
    {
        _logger = logger;
        _twilioProducer = twilioProducer;
    }
    public async Task<Result> Handle(CreateNotificationWhatsAppCommand request, CancellationToken cancellationToken)
    {
        _twilioProducer.PublishAsync(new NotificationTwilioEvent(request.NotificationType, request.From, request.Body, request.To));
        return Result.SuccessWithMessage("Mensagem enviada com sucesso!");
    }
}
