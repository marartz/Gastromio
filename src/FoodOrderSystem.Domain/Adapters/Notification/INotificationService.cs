using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Domain.Adapters.Notification
{
    public interface INotificationService
    {
        Task<NotificationResponse> SendNotificationAsync(NotificationRequest notificationRequest,
            CancellationToken cancellationToken = default);
    }
}