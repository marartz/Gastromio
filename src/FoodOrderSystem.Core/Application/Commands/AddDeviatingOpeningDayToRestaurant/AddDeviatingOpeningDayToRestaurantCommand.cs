using System;
using FoodOrderSystem.Core.Common;
using FoodOrderSystem.Core.Domain.Model.Restaurant;

namespace FoodOrderSystem.Core.Application.Commands.AddDeviatingOpeningDayToRestaurant
{
    public class AddDeviatingOpeningDayToRestaurantCommand : ICommand<bool>
    {
        public AddDeviatingOpeningDayToRestaurantCommand(RestaurantId restaurantId, Date date)
        {
            RestaurantId = restaurantId;
            Date = date;
        }
        
        public RestaurantId RestaurantId { get; }
        public Date Date { get; }
    }
}