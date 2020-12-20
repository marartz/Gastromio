using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gastromio.Core.Application.Ports.Notification;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace Gastromio.Notification.Smtp
{
    public class SmtpNotificationService : INotificationService
    {
        private readonly ILogger<SmtpNotificationService> logger;
        private readonly SmtpConfiguration configuration;

        public SmtpNotificationService(ILogger<SmtpNotificationService> logger, SmtpConfiguration configuration)
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

            if (string.IsNullOrEmpty(configuration.ServerName) || string.IsNullOrEmpty(configuration.UserName) ||
                string.IsNullOrEmpty(configuration.Password))
            {
                logger.LogWarning("Skipped sending mail due to missing SMTP configuration");
                return new NotificationResponse(true, "skipped due to missing SMTP configuration");
            }

            try
            {
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

                var mimeMessage = new MimeMessage();

                mimeMessage.From.Add(new MailboxAddress(notificationRequest.Sender.Name,
                    notificationRequest.Sender.Email));

                if (notificationRequest.RecipientsTo != null)
                {
                    foreach (var recipient in notificationRequest.RecipientsTo)
                    {
                        mimeMessage.To.Add(new MailboxAddress(recipient.Name, recipient.Email));
                    }
                }

                if (notificationRequest.RecipientsCc != null)
                {
                    foreach (var recipient in notificationRequest.RecipientsCc)
                    {
                        mimeMessage.Cc.Add(new MailboxAddress(recipient.Name, recipient.Email));
                    }
                }

                if (notificationRequest.RecipientsBcc != null)
                {
                    foreach (var recipient in notificationRequest.RecipientsBcc)
                    {
                        mimeMessage.Bcc.Add(new MailboxAddress(recipient.Name, recipient.Email));
                    }
                }

                mimeMessage.Subject = notificationRequest.Subject;

                var builder = new BodyBuilder
                {
                    TextBody = notificationRequest.TextPart,
                    HtmlBody = notificationRequest.HtmlPart
                };

                mimeMessage.Body = builder.ToMessageBody();

                using var smtpClient = new SmtpClient
                {
                    ServerCertificateValidationCallback = (s, c, h, e) => true
                };

                await smtpClient.ConnectAsync(configuration.ServerName, configuration.Port,
                    SecureSocketOptions.StartTlsWhenAvailable, cancellationToken);
                await smtpClient.AuthenticateAsync(configuration.UserName, configuration.Password, cancellationToken);
                await smtpClient.SendAsync(mimeMessage, cancellationToken);
                await smtpClient.DisconnectAsync(true, cancellationToken);

                logger.LogInformation("Successfully sent");
                return new NotificationResponse(true, null);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error during sending email by SMTP");
                return new NotificationResponse(false, e.Message);
            }
        }
    }
}