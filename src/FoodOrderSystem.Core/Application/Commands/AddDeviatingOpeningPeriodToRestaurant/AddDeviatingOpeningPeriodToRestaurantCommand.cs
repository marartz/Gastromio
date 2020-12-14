using System;
using FoodOrderSystem.Core.Domain.Model.Restaurant;

namespace FoodOrderSystem.Core.Application.Commands.AddDeviatingOpeningPeriodToRestaurant
{
    public class AddDeviatingOpeningPeriodToRestaurantCommand : ICommand<bool>
    {
        public AddDeviatingOpeningPeriodToRestaurantCommand(RestaurantId restaurantId, DateTime date, TimeSpan start, TimeSpan end)
        {
            RestaurantId = restaurantId;
            Date = date;
            Start = start;
            End = end;
        }
        
        public RestaurantId RestaurantId { get; }
        public DateTime Date { get; }
        public TimeSpan Start { get; }
        public TimeSpan End { get; }
    }
}