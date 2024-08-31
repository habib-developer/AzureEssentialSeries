using Lib.Net.Http.WebPush.Authentication;
using Lib.Net.Http.WebPush;

namespace Ep13.Services
{
    public class PushNotificationService
    {
        private readonly PushServiceClient _pushClient;
        private readonly VapidAuthentication _vapidAuthentication;

        public PushNotificationService(IConfiguration configuration)
        {
            _pushClient = new PushServiceClient
            {
                DefaultAuthentication = new VapidAuthentication(configuration["Push:PublicKey"], configuration["Push:PrivateKey"])

            };

            _vapidAuthentication = _pushClient.DefaultAuthentication;
        }

        public async Task SendNotificationAsync(PushSubscription subscription, string payload)
        {
            try
            {
                var pushMessage = new PushMessage(payload);
                await _pushClient.RequestPushMessageDeliveryAsync(subscription, pushMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending notification: {ex.Message}");
            }
        }
    }
}
