using Gastromio.Core.Application.Ports.Notification;
using Microsoft.Extensions.DependencyInjection;

namespace Gastromio.Notification.Smtp
{
    public static class ServiceCollectionExtensions
    {
        public static void AddEmailNotificationViaSmtp(this IServiceCollection services)
        {
            services.AddTransient<IEmailNotificationService, SmtpEmailNotificationService>();
        }
    }
}