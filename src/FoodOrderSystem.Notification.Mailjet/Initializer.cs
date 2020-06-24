using FoodOrderSystem.Domain.Adapters.Notification;
using Microsoft.Extensions.DependencyInjection;

namespace FoodOrderSystem.Notification.Mailjet
{
    public static class Initializer
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<INotificationService, MailjetNotificationService>();
        }
    }
}