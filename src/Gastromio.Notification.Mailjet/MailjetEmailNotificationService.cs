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
    public class MailjetEmailNotificationService : IEmailNotificationService
    {
        private readonly ILogger<MailjetEmailNotificationService> logger;
        private readonly MailjetEmailConfiguration configuration;

        public MailjetEmailNotificationService(ILogger<MailjetEmailNotificationService> logger,
            MailjetEmailConfiguration configuration)
        {
            this.logger = logger;
            this.configuration = configuration;
        }

        public async Task<EmailNotificationResponse> SendEmailNotificationAsync(EmailNotificationRequest emailNotificationRequest,
            CancellationToken cancellationToken = default)
        {
            if (emailNotificationRequest == null)
                throw new ArgumentNullException(nameof(emailNotificationRequest));

            if (emailNotificationRequest.Sender == null)
                throw new InvalidOperationException("no sender specified");

            if (emailNotificationRequest.RecipientsTo == null)
                throw new InvalidOperationException("no recipient (to) specified");

            if (emailNotificationRequest.Subject == null)
                throw new InvalidOperationException("no subject specified");

            if (emailNotificationRequest.HtmlPart == null ^ emailNotificationRequest.TextPart == null)
                throw new InvalidOperationException("either html or text part has to be specified");

            var message = new JObject
            {
                {
                    "From",
                    new JObject {{"Email", emailNotificationRequest.Sender.Email}, {"Name", emailNotificationRequest.Sender.Name}}
                },
                {"Subject", emailNotificationRequest.Subject},
                {"TextPart", emailNotificationRequest.TextPart},
                {"HTMLPart", emailNotificationRequest.HtmlPart}
            };

            if (emailNotificationRequest.RecipientsTo != null)
            {
                var array = new JArray();
                foreach (var recipient in emailNotificationRequest.RecipientsTo)
                {
                    array.Add(new JObject {{"Email", recipient.Email}, {"Name", recipient.Name}});
                }

                message.Add("To", array);
            }

            if (emailNotificationRequest.RecipientsCc != null)
            {
                var array = new JArray();
                foreach (var recipient in emailNotificationRequest.RecipientsCc)
                {
                    array.Add(new JObject {{"Email", recipient.Email}, {"Name", recipient.Name}});
                }

                message.Add("Cc", array);
            }

            if (emailNotificationRequest.RecipientsBcc != null)
            {
                var array = new JArray();
                foreach (var recipient in emailNotificationRequest.RecipientsBcc)
                {
                    array.Add(new JObject {{"Email", recipient.Email}, {"Name", recipient.Name}});
                }

                message.Add("Bcc", array);
            }

            var request = new MailjetRequest {Resource = Send.Resource}.Property(Send.Messages, new JArray {message});

            logger.LogInformation("Sending email from {0} to {1} (cc: {2}, bcc: {3}) with subject {4}",
                emailNotificationRequest.Sender.Email,
                string.Join("; ", emailNotificationRequest.RecipientsTo.Select(en => en.Email)),
                emailNotificationRequest.RecipientsCc != null && emailNotificationRequest.RecipientsCc.Count > 0
                    ? string.Join("; ", emailNotificationRequest.RecipientsCc.Select(en => en.Email))
                    : "n/a",
                emailNotificationRequest.RecipientsBcc != null && emailNotificationRequest.RecipientsBcc.Count > 0
                    ? string.Join("; ", emailNotificationRequest.RecipientsBcc.Select(en => en.Email))
                    : "n/a",
                emailNotificationRequest.Subject
            );

            if (string.IsNullOrEmpty(configuration.ApiKey) || string.IsNullOrEmpty(configuration.ApiSecret))
            {
                logger.LogWarning("Skipped sending mail due to missing Mailjet configuration");
                return new EmailNotificationResponse(true, "skipped due to missing Mailjet configuration");
            }

            var client = new MailjetClient(configuration.ApiKey, configuration.ApiSecret);
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
                ? new EmailNotificationResponse(true, null)
                : new EmailNotificationResponse(false, response.GetErrorMessage());
        }
    }
}
