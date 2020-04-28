using FoodOrderSystem.Domain.Model.Restaurant;
using System;

namespace FoodOrderSystem.Domain.Commands.AddDeliveryTimeToRestaurant
{
    public class AddDeliveryTimeToRestaurantCommand : ICommand
    {
        public AddDeliveryTimeToRestaurantCommand(RestaurantId restaurantId, int dayOfWeek, TimeSpan start, TimeSpan end)
        {
            RestaurantId = restaurantId;
            DayOfWeek = dayOfWeek;
            Start = start;
            End = end;
        }

        public RestaurantId RestaurantId { get; }
        public int DayOfWeek { get; }
        public TimeSpan Start { get; }
        public TimeSpan End { get; }
    }
}
