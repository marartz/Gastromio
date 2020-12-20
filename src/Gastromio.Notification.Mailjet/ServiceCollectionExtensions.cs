using Gastromio.Core.Application.Ports.Notification;
using Microsoft.Extensions.DependencyInjection;

namespace Gastromio.Notification.Mailjet
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMailjet(this IServiceCollection services)
        {
            services.AddTransient<INotificationService, MailjetNotificationService>();
        }
    }
}