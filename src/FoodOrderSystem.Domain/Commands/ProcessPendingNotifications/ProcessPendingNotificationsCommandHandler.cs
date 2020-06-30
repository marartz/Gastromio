using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FoodOrderSystem.Domain.Adapters.Notification;
using FoodOrderSystem.Domain.Adapters.Template;
using FoodOrderSystem.Domain.Model;
using FoodOrderSystem.Domain.Model.Order;
using FoodOrderSystem.Domain.Model.User;
using Microsoft.Extensions.Logging;

namespace FoodOrderSystem.Domain.Commands.ProcessPendingNotifications
{
    public class ProcessPendingNotificationsCommandHandler : ICommandHandler<ProcessPendingNotificationsCommand, bool>
    {
        private readonly ILogger<ProcessPendingNotificationsCommandHandler> logger;
        private readonly IOrderRepository orderRepository;
        private readonly ITemplateService templateService;
        private readonly INotificationService notificationService;

        public ProcessPendingNotificationsCommandHandler(
            ILogger<ProcessPendingNotificationsCommandHandler> logger,
            IOrderRepository orderRepository,
            ITemplateService templateService,
            INotificationService notificationService
        )
        {
            this.logger = logger;
            this.orderRepository = orderRepository;
            this.templateService = templateService;
            this.notificationService = notificationService;
        }

        public async Task<Result<bool>> HandleAsync(ProcessPendingNotificationsCommand command, User currentUser,
            CancellationToken cancellationToken = default)
        {
            logger.LogDebug("Starting check for pending notifications");
            
            var pendingCustomerNotificationOrders =
                await orderRepository.FindByPendingCustomerNotificationAsync(cancellationToken);

            foreach (var order in pendingCustomerNotificationOrders)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;
                
                await TriggerCustomerNotificationAsync(order, cancellationToken);
            }
            
            var pendingRestaurantNotificationOrders =
                await orderRepository.FindByPendingRestaurantNotificationAsync(cancellationToken);

            foreach (var order in pendingRestaurantNotificationOrders)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;
                await TriggerRestaurantNotificationAsync(order, cancellationToken);
            }
            
            logger.LogDebug("Check for pending notifications ended");
            return SuccessResult<bool>.Create(true);
        }

        private async Task TriggerCustomerNotificationAsync(Order order,
            CancellationToken cancellationToken)
        {
            if (order.CustomerNotificationInfo != null && order.CustomerNotificationInfo.Attempt > 0)
            {
                var delay = CalculateDelay(order.CustomerNotificationInfo);
                var delta = DateTime.UtcNow - order.CustomerNotificationInfo.Timestamp;
                if (delta < delay)
                {
                    logger.LogDebug("Delay sending customer email of order {0} by further {1} seconds", order.Id.Value, (delay - delta).TotalSeconds);
                    return;
                }
            }
            
            var emailData = templateService.GetCustomerEmail(order);
            if (emailData == null)
            {
                logger.LogError("Could not retrieve email data for customer email");
                throw new InvalidOperationException("could not retrieve email data for customer email");
            }

            var notificationRequest = new NotificationRequest(
                new EmailAddress("Gastromio.de", "noreply@gastromio.de"),
                new List<EmailAddress>
                {
                    new EmailAddress($"{order.CustomerInfo.GivenName} {order.CustomerInfo.LastName}", order.CustomerInfo.Email)
                },
                new List<EmailAddress>(),
                new List<EmailAddress>(),
                emailData.Subject,
                emailData.TextPart,
                emailData.HtmlPart
            );
            
            var (success, info) = await TriggerNotificationAsync(notificationRequest, cancellationToken);
            order.RegisterCustomerNotificationAttempt(success, info);
            await orderRepository.StoreAsync(order, cancellationToken);
        }
        
        private async Task TriggerRestaurantNotificationAsync(Order order,
            CancellationToken cancellationToken)
        {
            if (order.RestaurantNotificationInfo != null && order.RestaurantNotificationInfo.Attempt > 0)
            {
                var delay = CalculateDelay(order.RestaurantNotificationInfo);
                var delta = DateTime.UtcNow - order.RestaurantNotificationInfo.Timestamp;
                if (delta < delay)
                {
                    logger.LogDebug("Delay sending restaurant email of order {0} by further {1} seconds", order.Id.Value, (delay - delta).TotalSeconds);
                    return;
                }
            }
            
            var emailData = templateService.GetRestaurantEmail(order);
            if (emailData == null)
            {
                logger.LogError("Could not retrieve email data for restaurant email");
                throw new InvalidOperationException("could not retrieve email data for restaurant email");
            }
            
            var recipientsCc = new List<EmailAddress>();
            if (order.CartInfo.RestaurantNeedsSupport)
            {
                recipientsCc.Add(new EmailAddress("Gastromio-Bestellungen", "bestellungen@gastromio.de"));
            }

            var notificationRequest = new NotificationRequest(
                new EmailAddress("Gastromio.de", "noreply@gastromio.de"),
                new List<EmailAddress>
                {
                    new EmailAddress($"{order.CartInfo.RestaurantName}", order.CartInfo.RestaurantEmail)
                },
                recipientsCc,
                new List<EmailAddress>(),
                emailData.Subject,
                emailData.TextPart,
                emailData.HtmlPart
            );
            
            var (success, info) = await TriggerNotificationAsync(notificationRequest, cancellationToken);
            order.RegisterRestaurantNotificationAttempt(success, info);
            await orderRepository.StoreAsync(order, cancellationToken);
        }
        
        private async Task<(bool success, string info)> TriggerNotificationAsync(
            NotificationRequest notificationRequest, CancellationToken cancellationToken)
        {
            try
            {
                var notificationResponse = await notificationService.SendNotificationAsync(notificationRequest, cancellationToken);
                return notificationResponse.Success
                    ? (true, notificationResponse.Message)
                    : (false, notificationResponse.Message);
            }
            catch (Exception e)
            {
                logger.LogWarning(e, "exception while sending notification");
                return (false, "Exception: " + e);
            }
        }

        private TimeSpan CalculateDelay(NotificationInfo notificationInfo)
        {
            var seconds = (int) Math.Pow(1.5, notificationInfo.Attempt);
            if (seconds > 600) // 10 minutes
                seconds = 600;
            return TimeSpan.FromSeconds(seconds);
        }
    }
}