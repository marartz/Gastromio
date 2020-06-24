using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FoodOrderSystem.Domain.Adapters.Notification;
using FoodOrderSystem.Domain.Adapters.Template;
using FoodOrderSystem.Domain.Model;
using FoodOrderSystem.Domain.Model.Order;
using FoodOrderSystem.Domain.Model.PaymentMethod;
using FoodOrderSystem.Domain.Model.Restaurant;
using FoodOrderSystem.Domain.Model.User;
using Microsoft.Extensions.Logging;

namespace FoodOrderSystem.Domain.Commands.ProcessPendingNotifications
{
    public class ProcessPendingNotificationsCommandHandler : ICommandHandler<ProcessPendingNotificationsCommand, bool>
    {
        private readonly ILogger<ProcessPendingNotificationsCommandHandler> logger;
        private readonly IOrderRepository orderRepository;
        private readonly IRestaurantRepository restaurantRepository;
        private readonly IPaymentMethodRepository paymentMethodRepository;
        private readonly ITemplateService templateService;
        private readonly INotificationService notificationService;

        public ProcessPendingNotificationsCommandHandler(
            ILogger<ProcessPendingNotificationsCommandHandler> logger,
            IOrderRepository orderRepository,
            IRestaurantRepository restaurantRepository,
            IPaymentMethodRepository paymentMethodRepository,
            ITemplateService templateService,
            INotificationService notificationService
        )
        {
            this.logger = logger;
            this.orderRepository = orderRepository;
            this.restaurantRepository = restaurantRepository;
            this.paymentMethodRepository = paymentMethodRepository;
            this.templateService = templateService;
            this.notificationService = notificationService;
        }

        public async Task<Result<bool>> HandleAsync(ProcessPendingNotificationsCommand command, User currentUser,
            CancellationToken cancellationToken = default)
        {
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
            
            return SuccessResult<bool>.Create(true);
        }

        private async Task TriggerCustomerNotificationAsync(Order order,
            CancellationToken cancellationToken)
        {
            var restaurant =
                await restaurantRepository.FindByRestaurantIdAsync(order.CartInfo.RestaurantId, cancellationToken);

            var paymentMethod =
                await paymentMethodRepository.FindByPaymentMethodIdAsync(order.PaymentMethodId, cancellationToken);

            var emailData = templateService.GetCustomerEmail(order);
            if (emailData == null)
                return;

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
            var restaurant =
                await restaurantRepository.FindByRestaurantIdAsync(order.CartInfo.RestaurantId, cancellationToken);

            var paymentMethod =
                await paymentMethodRepository.FindByPaymentMethodIdAsync(order.PaymentMethodId, cancellationToken);

            var emailData = templateService.GetRestaurantEmail(order);
            if (emailData == null)
                return;

            var notificationRequest = new NotificationRequest(
                new EmailAddress("Gastromio.de", "noreply@gastromio.de"),
                new List<EmailAddress>
                {
                    new EmailAddress($"{restaurant.Name}", restaurant.ContactInfo.EmailAddress)
                },
                new List<EmailAddress>(),
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
                return (false, "Exception: " + e);
            }
        }
    }
}