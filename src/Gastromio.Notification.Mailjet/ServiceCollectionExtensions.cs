using System.Runtime.CompilerServices;
using Gastromio.Core.Application.Ports.Notification;
using Microsoft.Extensions.DependencyInjection;

[assembly:InternalsVisibleTo("Gastromio.Notification.Mailjet.Tests")]

namespace Gastromio.Notification.Mailjet
{
    public static class ServiceCollectionExtensions
    {
        public static void AddEmailNotificationViaMailjet(this IServiceCollection services)
        {
            services.AddTransient<IEmailNotificationService, MailjetEmailNotificationService>();
        }

        public static void AddMobileNotificationViaMailjet(this IServiceCollection services)
        {
            services.AddTransient<IMobileNotificationService, MailjetMobileNotificationService>();
        }
    }
}
