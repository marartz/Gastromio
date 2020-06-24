using System.Threading;
using System.Threading.Tasks;
using FoodOrderSystem.Domain.Adapters.Notification;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace FoodOrderSystem.Notification.Mailjet
{
    public class MailjetNotificationService : INotificationService
    {
        private readonly ILogger<MailjetNotificationService> logger;
        private readonly MailjetConfiguration configuration;

        public MailjetNotificationService(ILogger<MailjetNotificationService> logger,
            MailjetConfiguration configuration)
        {
            this.logger = logger;
            this.configuration = configuration;
        }

        public async Task<NotificationResponse> SendNotificationAsync(NotificationRequest notificationRequest,
            CancellationToken cancellationToken = default)
        {
            var message = new JObject
            {
                {
                    "From",
                    new JObject {{"Email", notificationRequest.Sender.Email}, {"Name", notificationRequest.Sender.Name}}
                },
                {"Subject", notificationRequest.Subject},
                {"TextPart", notificationRequest.TextPart},
                {"HTMLPart", notificationRequest.HtmlPart}
            };

            if (notificationRequest.RecipientsTo != null)
            {
                var array = new JArray();
                foreach (var recipient in notificationRequest.RecipientsTo)
                {
                    array.Add(new JObject {{"Email", recipient.Email}, {"Name", recipient.Name}});
                }

                message.Add("To", array);
            }

            if (notificationRequest.RecipientsCc != null)
            {
                var array = new JArray();
                foreach (var recipient in notificationRequest.RecipientsCc)
                {
                    array.Add(new JObject {{"Email", recipient.Email}, {"Name", recipient.Name}});
                }

                message.Add("Cc", array);
            }

            if (notificationRequest.RecipientsBcc != null)
            {
                var array = new JArray();
                foreach (var recipient in notificationRequest.RecipientsBcc)
                {
                    array.Add(new JObject {{"Email", recipient.Email}, {"Name", recipient.Name}});
                }

                message.Add("Bcc", array);
            }

            var client = new MailjetClient(configuration.ApiKey, configuration.ApiSecret)
            {
                Version = ApiVersion.V3_1,
            };

            var request = new MailjetRequest {Resource = Send.Resource}.Property(Send.Messages, new JArray {message});

            var response = await client.PostAsync(request);
            return response.IsSuccessStatusCode
                ? new NotificationResponse(true, null)
                : new NotificationResponse(false, response.GetErrorMessage());
        }
    }
}