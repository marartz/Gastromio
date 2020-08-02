using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Core.Application.Ports.Notification
{
    public interface INotificationService
    {
        Task<NotificationResponse> SendNotificationAsync(NotificationRequest notificationRequest,
            CancellationToken cancellationToken = default);
    }
}