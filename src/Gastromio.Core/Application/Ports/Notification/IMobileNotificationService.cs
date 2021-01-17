using System.Threading;
using System.Threading.Tasks;

namespace Gastromio.Core.Application.Ports.Notification
{
    public interface IMobileNotificationService
    {
        Task<MobileNotificationResponse> SendMobileNotificationAsync(
            MobileNotificationRequest mobileNotificationRequest, CancellationToken cancellationToken = default);
    }
}