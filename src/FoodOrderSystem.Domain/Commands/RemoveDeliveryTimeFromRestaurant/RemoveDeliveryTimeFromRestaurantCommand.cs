using FoodOrderSystem.Domain.Model.Restaurant;
using System;

namespace FoodOrderSystem.Domain.Commands.RemoveDeliveryTimeFromRestaurant
{
    public class RemoveDeliveryTimeFromRestaurantCommand : ICommand
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
