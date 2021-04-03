using System.Threading;
using System.Threading.Tasks;

namespace Gastromio.Core.Application.Ports.Notification
{
    public interface IEmailNotificationService
    {
        Task<EmailNotificationResponse> SendEmailNotificationAsync(EmailNotificationRequest emailNotificationRequest,
            CancellationToken cancellationToken = default);
    }
}