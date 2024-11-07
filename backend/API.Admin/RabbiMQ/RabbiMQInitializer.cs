using API.Admin.Feature.Notification.Consumers;
using API.Admin.Feature.Notification.Producer;
using API.Admin.Feature.Notification.Service;
using poc.core.api.net8.Interface;

namespace API.Admin.RabbiMQ;

public class RabbiMQInitializer
{
    public static void Initialize(IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddScoped<IMessageBusService, MessageBusService>();
        services.AddScoped<ITwilioService, TwilioService>();

        // Publish
        services.AddScoped<ITwilioProducer, TwilioProducer>();

        // Subscribe
        // services.AddHostedService<TwilioWhatsAppConsumer>();
    }
}
