using API.Admin.Feature.Notification.Request;

namespace API.Admin.Feature.Notification.Service;

public interface ITwilioService
{
    Task TwilioAsync(TwilioRequest dto);
}
