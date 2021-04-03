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
    public class SmtpEmailNotificationService : IEmailNotificationService
    {
        private readonly ILogger<SmtpEmailNotificationService> logger;
        private readonly SmtpEmailConfiguration emailConfiguration;

        public SmtpEmailNotificationService(ILogger<SmtpEmailNotificationService> logger, SmtpEmailConfiguration emailConfiguration)
        {
            this.logger = logger;
            this.emailConfiguration = emailConfiguration;
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

            if (string.IsNullOrEmpty(emailConfiguration.ServerName) || string.IsNullOrEmpty(emailConfiguration.UserName) ||
                string.IsNullOrEmpty(emailConfiguration.Password))
            {
                logger.LogWarning("Skipped sending mail due to missing SMTP configuration");
                return new EmailNotificationResponse(true, "skipped due to missing SMTP configuration");
            }

            try
            {
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

                var mimeMessage = new MimeMessage();

                mimeMessage.From.Add(new MailboxAddress(emailNotificationRequest.Sender.Name,
                    emailNotificationRequest.Sender.Email));

                if (emailNotificationRequest.RecipientsTo != null)
                {
                    foreach (var recipient in emailNotificationRequest.RecipientsTo)
                    {
                        mimeMessage.To.Add(new MailboxAddress(recipient.Name, recipient.Email));
                    }
                }

                if (emailNotificationRequest.RecipientsCc != null)
                {
                    foreach (var recipient in emailNotificationRequest.RecipientsCc)
                    {
                        mimeMessage.Cc.Add(new MailboxAddress(recipient.Name, recipient.Email));
                    }
                }

                if (emailNotificationRequest.RecipientsBcc != null)
                {
                    foreach (var recipient in emailNotificationRequest.RecipientsBcc)
                    {
                        mimeMessage.Bcc.Add(new MailboxAddress(recipient.Name, recipient.Email));
                    }
                }

                mimeMessage.Subject = emailNotificationRequest.Subject;

                var builder = new BodyBuilder
                {
                    TextBody = emailNotificationRequest.TextPart,
                    HtmlBody = emailNotificationRequest.HtmlPart
                };

                mimeMessage.Body = builder.ToMessageBody();

                using var smtpClient = new SmtpClient
                {
                    ServerCertificateValidationCallback = (s, c, h, e) => true
                };

                await smtpClient.ConnectAsync(emailConfiguration.ServerName, emailConfiguration.Port,
                    SecureSocketOptions.StartTlsWhenAvailable, cancellationToken);
                await smtpClient.AuthenticateAsync(emailConfiguration.UserName, emailConfiguration.Password, cancellationToken);
                await smtpClient.SendAsync(mimeMessage, cancellationToken);
                await smtpClient.DisconnectAsync(true, cancellationToken);

                logger.LogInformation("Successfully sent");
                return new EmailNotificationResponse(true, null);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error during sending email by SMTP");
                return new EmailNotificationResponse(false, e.Message);
            }
        }
    }
}