using FoodOrderSystem.Domain.Model.Restaurant;
using FoodOrderSystem.Domain.ViewModels;
using System;

namespace FoodOrderSystem.Domain.Commands.RemoveDeliveryTimeFromRestaurant
{
    public class RemoveDeliveryTimeFromRestaurantCommand : ICommand<bool>
    {
        public RemoveDeliveryTimeFromRestaurantCommand(RestaurantId restaurantId, int dayOfWeek, TimeSpan start)
        {
            RestaurantId = restaurantId;
            DayOfWeek = dayOfWeek;
            Start = start;
        }

        public RestaurantId RestaurantId { get; }
        public int DayOfWeek { get; }
        public TimeSpan Start { get; }
    }
}
