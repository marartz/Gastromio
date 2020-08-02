using System;
using FoodOrderSystem.Core.Domain.Model.Restaurant;

namespace FoodOrderSystem.Core.Application.Commands.RemoveOpeningPeriodFromRestaurant
{
    public class RemoveOpeningPeriodFromRestaurantCommand : ICommand<bool>
    {
        public RemoveOpeningPeriodFromRestaurantCommand(RestaurantId restaurantId, int dayOfWeek, TimeSpan start)
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
