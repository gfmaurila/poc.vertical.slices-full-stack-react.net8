using API.Admin.Feature.Notification.Events;

namespace API.Admin.Feature.Notification.Producer;

public interface ITwilioProducer
{
    void PublishAsync(NotificationTwilioEvent evento);
}
