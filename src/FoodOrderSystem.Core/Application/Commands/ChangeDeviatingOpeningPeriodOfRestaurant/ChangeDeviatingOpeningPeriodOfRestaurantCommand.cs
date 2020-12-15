using System;
using FoodOrderSystem.Core.Common;
using FoodOrderSystem.Core.Domain.Model.Restaurant;

namespace FoodOrderSystem.Core.Application.Commands.ChangeDeviatingOpeningPeriodOfRestaurant
{
    public class ChangeDeviatingOpeningPeriodOfRestaurantCommand : ICommand<bool>
    {
        public ChangeDeviatingOpeningPeriodOfRestaurantCommand(RestaurantId restaurantId, Date date, TimeSpan oldStart, TimeSpan newStart, TimeSpan newEnd)
        {
            RestaurantId = restaurantId;
            Date = date;
            OldStart = oldStart;
            NewStart = newStart;
            NewEnd = newEnd;
        }
        
        public RestaurantId RestaurantId { get; }
        public Date Date { get; }
        public TimeSpan OldStart { get; }
        public TimeSpan NewStart { get; }
        public TimeSpan NewEnd { get; }
    }
}