using System.Collections.Generic;

namespace FoodOrderSystem.Domain.Adapters.Notification
{
    public class NotificationRequest
    {
        public NotificationRequest(
            EmailAddress sender,
            ICollection<EmailAddress> recipientsTo,
            ICollection<EmailAddress> recipientsCc,
            ICollection<EmailAddress> recipientsBcc,
            string subject,
            string textPart,
            string htmlPart
        )
        {
            Sender = sender;
            RecipientsTo = recipientsTo;
            RecipientsCc = recipientsCc;
            RecipientsBcc = recipientsBcc;
            Subject = subject;
            TextPart = textPart;
            HtmlPart = htmlPart;
        }
        
        public EmailAddress Sender { get; }
        
        public ICollection<EmailAddress> RecipientsTo { get; }
        
        public ICollection<EmailAddress> RecipientsCc { get; }
        
        public ICollection<EmailAddress> RecipientsBcc { get; }
        
        public string Subject { get; }
        
        public string TextPart { get; }
        
        public string HtmlPart { get; }
    }
}