using System;
using System.Linq;
using FoodOrderSystem.Domain.Model.PaymentMethod;
using FoodOrderSystem.Domain.Model.User;

namespace FoodOrderSystem.Domain.Model.Order
{
    public class Order
    {
        public Order(
            OrderId id,
            CustomerInfo customerInfo,
            CartInfo cartInfo,
            string comments,
            PaymentMethodId paymentMethodId,
            string paymentMethodName,
            string paymentMethodDescription,
            decimal costs,
            decimal totalPrice 
        )
        {
            Id = id;
            CustomerInfo = customerInfo;
            CartInfo = cartInfo;
            Comments = comments?.Trim();
            PaymentMethodId = paymentMethodId;
            PaymentMethodName = paymentMethodName;
            PaymentMethodDescription = paymentMethodDescription;
            Costs = costs;
            TotalPrice = totalPrice;
            CreatedOn = DateTime.UtcNow;
        }

        public Order(
            OrderId id,
            CustomerInfo customerInfo,
            CartInfo cartInfo,
            string comments,
            PaymentMethodId paymentMethodId,
            string paymentMethodName,
            string paymentMethodDescription,
            decimal costs,
            decimal totalPrice,
            NotificationInfo customerNotificationInfo,
            NotificationInfo restaurantNotificationInfo,
            DateTime createdOn,
            DateTime? updatedOn,
            UserId updatedBy
        )
        {
            Id = id;
            CustomerInfo = customerInfo;
            CartInfo = cartInfo;
            Comments = comments;
            PaymentMethodId = paymentMethodId;
            PaymentMethodName = paymentMethodName;
            PaymentMethodDescription = paymentMethodDescription;
            Costs = costs;
            TotalPrice = totalPrice;
            CustomerNotificationInfo = customerNotificationInfo;
            RestaurantNotificationInfo = restaurantNotificationInfo;
            CreatedOn = createdOn;
            UpdatedOn = updatedOn;
            UpdatedBy = updatedBy;
        }

        public OrderId Id { get; }

        public CustomerInfo CustomerInfo { get; }

        public CartInfo CartInfo { get; }

        public string Comments { get; }

        public PaymentMethodId PaymentMethodId { get; }

        public string PaymentMethodName { get; }

        public string PaymentMethodDescription { get; }

        public decimal Costs { get; }

        public decimal TotalPrice { get; }

        public NotificationInfo CustomerNotificationInfo { get; private set; }

        public NotificationInfo RestaurantNotificationInfo { get; private set; }

        public DateTime CreatedOn { get; }

        public DateTime? UpdatedOn { get; private set; }

        public UserId UpdatedBy { get; private set; }

        public Result<bool> Validate()
        {
            if (CustomerInfo == null)
                return FailureResult<bool>.Create(FailureResultCode.OrderIsInvalid);

            if (string.IsNullOrWhiteSpace(CustomerInfo.GivenName))
                return FailureResult<bool>.Create(FailureResultCode.OrderIsInvalid);

            if (string.IsNullOrWhiteSpace(CustomerInfo.LastName))
                return FailureResult<bool>.Create(FailureResultCode.OrderIsInvalid);

            if (string.IsNullOrWhiteSpace(CustomerInfo.Street))
                return FailureResult<bool>.Create(FailureResultCode.OrderIsInvalid);

            if (string.IsNullOrWhiteSpace(CustomerInfo.ZipCode))
                return FailureResult<bool>.Create(FailureResultCode.OrderIsInvalid);

            if (string.IsNullOrWhiteSpace(CustomerInfo.City))
                return FailureResult<bool>.Create(FailureResultCode.OrderIsInvalid);

            if (string.IsNullOrWhiteSpace(CustomerInfo.Phone))
                return FailureResult<bool>.Create(FailureResultCode.OrderIsInvalid);

            if (string.IsNullOrWhiteSpace(CustomerInfo.Email))
                return FailureResult<bool>.Create(FailureResultCode.OrderIsInvalid);

            if (CartInfo == null)
                return FailureResult<bool>.Create(FailureResultCode.OrderIsInvalid);

            if (!string.IsNullOrWhiteSpace(Comments) && Comments.Length > 1000)
                return FailureResult<bool>.Create(FailureResultCode.OrderIsInvalid);

            return SuccessResult<bool>.Create(true);
        }

        public decimal GetValueOfOrder()
        {
            return CartInfo?.OrderedDishes?.Sum(orderedDish => orderedDish.Count * orderedDish.VariantPrice) ?? 0;
        }

        public decimal GetTotalPrice()
        {
            return GetValueOfOrder() + Costs;
        }

        public Result<bool> RegisterCustomerNotificationAttempt(bool status, string message)
        {
            CustomerNotificationInfo = CustomerNotificationInfo == null
                ? new NotificationInfo(status, 1, message, DateTime.UtcNow)
                : new NotificationInfo(status, CustomerNotificationInfo.Attempt + 1, message, DateTime.UtcNow);
            UpdatedOn = DateTime.UtcNow;

            return SuccessResult<bool>.Create(true);
        }

        public Result<bool> RegisterRestaurantNotificationAttempt(bool status, string message)
        {
            RestaurantNotificationInfo = RestaurantNotificationInfo == null
                ? new NotificationInfo(status, 1, message, DateTime.UtcNow)
                : new NotificationInfo(status, RestaurantNotificationInfo.Attempt + 1, message, DateTime.UtcNow);
            UpdatedOn = DateTime.UtcNow;

            return SuccessResult<bool>.Create(true);
        }
    }
}