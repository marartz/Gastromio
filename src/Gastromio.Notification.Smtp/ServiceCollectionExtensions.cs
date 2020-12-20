using Gastromio.Core.Application.Ports.Notification;
using Microsoft.Extensions.DependencyInjection;

namespace Gastromio.Notification.Smtp
{
    public static class ServiceCollectionExtensions
    {
        public static void AddSmtp(this IServiceCollection services)
        {
            services.AddTransient<INotificationService, SmtpNotificationService>();
        }
    }
}