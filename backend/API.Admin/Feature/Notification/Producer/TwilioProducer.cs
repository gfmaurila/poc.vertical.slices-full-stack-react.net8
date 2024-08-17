﻿using API.Admin.Feature.Notification.Events;
using API.Admin.RabbiMQ;
using poc.core.api.net8.Enumerado;
using poc.core.api.net8.Interface;
using System.Text;
using System.Text.Json;

namespace API.Admin.Feature.Notification.Producer;

public class TwilioProducer : ITwilioProducer
{
    private readonly IMessageBusService _messageBusService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<TwilioProducer> _logger;
    public TwilioProducer(IMessageBusService messageBusService, IConfiguration configuration, ILogger<TwilioProducer> logger)
    {
        _messageBusService = messageBusService;
        _configuration = configuration;
        _logger = logger;
    }

    public void PublishAsync(NotificationTwilioEvent notification)
    {
        string QUEUE_NAME = "";

        if (notification.NotificationType == ENotificationType.WhatsApp)
            QUEUE_NAME = _configuration.GetValue<string>(RabbiMQConsts.QUEUE_TWILIO_WHATSAPP);

        if (notification.NotificationType == ENotificationType.SMS)
            QUEUE_NAME = _configuration.GetValue<string>(RabbiMQConsts.QUEUE_TWILIO_SMS);

        var notificationInfoJson = JsonSerializer.Serialize(notification);
        var notificationInfoBytes = Encoding.UTF8.GetBytes(notificationInfoJson);
        _messageBusService.Publish(QUEUE_NAME, notificationInfoBytes);
    }
}
