using System;
using FoodOrderSystem.Core.Common;
using FoodOrderSystem.Core.Domain.Model.Restaurant;

namespace FoodOrderSystem.Core.Application.Commands.RemoveDeviatingOpeningPeriodFromRestaurant
{
    public class RemoveDeviatingOpeningPeriodFromRestaurantCommand : ICommand<bool>
    {
        public RemoveDeviatingOpeningPeriodFromRestaurantCommand(RestaurantId restaurantId, Date date, TimeSpan start)
        {
            RestaurantId = restaurantId;
            Date = date;
            Start = start;
        }

        public RestaurantId RestaurantId { get; }
        public Date Date { get; }
        public TimeSpan Start { get; }
    }
}