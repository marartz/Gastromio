using System.Threading;
using System.Threading.Tasks;

namespace Gastromio.Core.Application.Ports.Notification
{
    public interface INotificationService
    {
        Task<NotificationResponse> SendNotificationAsync(NotificationRequest notificationRequest,
            CancellationToken cancellationToken = default);
    }
}