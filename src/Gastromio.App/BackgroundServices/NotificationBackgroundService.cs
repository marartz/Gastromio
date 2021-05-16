using System.Threading;
using System.Threading.Tasks;
using Gastromio.Core.Application.Commands;
using Gastromio.Core.Application.Commands.ProcessPendingNotifications;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Gastromio.App.BackgroundServices
{
    public class NotificationBackgroundService : BackgroundService
    {
        private readonly ILogger<NotificationBackgroundService> logger;
        private readonly ICommandDispatcher commandDispatcher;

        public NotificationBackgroundService(
            ILogger<NotificationBackgroundService> logger,
            ICommandDispatcher commandDispatcher
        )
        {
            this.logger = logger;
            this.commandDispatcher = commandDispatcher;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogDebug($"NotificationBackgroundService is starting.");

            stoppingToken.Register(() =>
                logger.LogDebug($"NotificationBackgroundService is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                //logger.LogDebug($"Checking orders for pending notifications");
                await CheckOrdersAsync(stoppingToken);
                await Task.Delay(5000, stoppingToken);
            }

            logger.LogDebug($"NotificationBackgroundService is stopping.");
        }

        private async Task CheckOrdersAsync(CancellationToken cancellationToken)
        {
            var command = new ProcessPendingNotificationsCommand();
            await commandDispatcher.PostAsync(command, null, cancellationToken);
        }
    }
}
