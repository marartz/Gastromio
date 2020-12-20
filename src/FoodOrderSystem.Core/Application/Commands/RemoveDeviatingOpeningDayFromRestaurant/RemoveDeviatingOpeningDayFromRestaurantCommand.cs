using System;
using FoodOrderSystem.Core.Common;
using FoodOrderSystem.Core.Domain.Model.Restaurant;

namespace FoodOrderSystem.Core.Application.Commands.RemoveDeviatingOpeningDayFromRestaurant
{
    public class RemoveDeviatingOpeningDayFromRestaurantCommand : ICommand<bool>
    {
        public RemoveDeviatingOpeningDayFromRestaurantCommand(RestaurantId restaurantId, Date date)
        {
            RestaurantId = restaurantId;
            Date = date;
        }
        
        public RestaurantId RestaurantId { get; }
        public Date Date { get; }
    }
}