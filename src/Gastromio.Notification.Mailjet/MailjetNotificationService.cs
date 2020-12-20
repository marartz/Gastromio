using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gastromio.Core.Application.Ports.Notification;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace Gastromio.Notification.Mailjet
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
            if (notificationRequest == null)
                throw new ArgumentNullException(nameof(notificationRequest));

            if (notificationRequest.Sender == null)
                throw new InvalidOperationException("no sender specified");

            if (notificationRequest.RecipientsTo == null)
                throw new InvalidOperationException("no recipient (to) specified");

            if (notificationRequest.Subject == null)
                throw new InvalidOperationException("no subject specified");

            if (notificationRequest.HtmlPart == null ^ notificationRequest.TextPart == null)
                throw new InvalidOperationException("either html or text part has to be specified");

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

            var request = new MailjetRequest {Resource = Send.Resource}.Property(Send.Messages, new JArray {message});

            logger.LogInformation("Sending email from {0} to {1} (cc: {2}, bcc: {3}) with subject {4}",
                notificationRequest.Sender.Email,
                string.Join("; ", notificationRequest.RecipientsTo.Select(en => en.Email)),
                notificationRequest.RecipientsCc != null && notificationRequest.RecipientsCc.Count > 0
                    ? string.Join("; ", notificationRequest.RecipientsCc.Select(en => en.Email))
                    : "n/a",
                notificationRequest.RecipientsBcc != null && notificationRequest.RecipientsBcc.Count > 0
                    ? string.Join("; ", notificationRequest.RecipientsBcc.Select(en => en.Email))
                    : "n/a",
                notificationRequest.Subject
            );

            if (string.IsNullOrEmpty(configuration.ApiKey) || string.IsNullOrEmpty(configuration.ApiSecret))
            {
                logger.LogWarning("Skipped sending mail due to missing Mailjet configuration");
                return new NotificationResponse(true, "skipped due to missing Mailjet configuration");
            }

            var client = new MailjetClient(configuration.ApiKey, configuration.ApiSecret)
            {
                Version = ApiVersion.V3_1,
            };

            var response = await client.PostAsync(request);

            if (response.IsSuccessStatusCode)
            {
                logger.LogInformation("Successfully sent");
            }
            else
            {
                logger.LogInformation("Mailjet returned error: {0}", response.GetErrorMessage());
            }

            return response.IsSuccessStatusCode
                ? new NotificationResponse(true, null)
                : new NotificationResponse(false, response.GetErrorMessage());
        }
    }
}