using System;
using FoodOrderSystem.Core.Domain.Model.Restaurant;

namespace FoodOrderSystem.Core.Application.Commands.AddOpeningPeriodToRestaurant
{
    public class AddOpeningPeriodToRestaurantCommand : ICommand<bool>
    {
        public AddOpeningPeriodToRestaurantCommand(RestaurantId restaurantId, int dayOfWeek, TimeSpan start, TimeSpan end)
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
