using FoodOrderSystem.Domain.Model.PaymentMethod;
using FoodOrderSystem.Domain.Model.Restaurant;
using FoodOrderSystem.Domain.ViewModels;
using System;

namespace FoodOrderSystem.Domain.Commands.AddPaymentMethodToRestaurant
{
    public class AddPaymentMethodToRestaurantCommand : ICommand<bool>
    {
        public AddPaymentMethodToRestaurantCommand(RestaurantId restaurantId, PaymentMethodId paymentMethodId)
        {
            RestaurantId = restaurantId;
            PaymentMethodId = paymentMethodId;
        }

        public RestaurantId RestaurantId { get; }
        public PaymentMethodId PaymentMethodId { get; }
    }
}
