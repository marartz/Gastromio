using System;
using FoodOrderSystem.Core.Domain.Model.Restaurant;

namespace FoodOrderSystem.Core.Application.Commands.RemoveRegularOpeningPeriodFromRestaurant
{
    public class RemoveRegularOpeningPeriodFromRestaurantCommand : ICommand<bool>
    {
        public RemoveRegularOpeningPeriodFromRestaurantCommand(RestaurantId restaurantId, int dayOfWeek, TimeSpan start)
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
