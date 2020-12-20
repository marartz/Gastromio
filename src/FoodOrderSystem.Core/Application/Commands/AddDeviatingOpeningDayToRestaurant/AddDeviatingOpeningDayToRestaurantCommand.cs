using System;
using FoodOrderSystem.Core.Common;
using FoodOrderSystem.Core.Domain.Model.Restaurant;

namespace FoodOrderSystem.Core.Application.Commands.AddDeviatingOpeningDayToRestaurant
{
    public class AddDeviatingOpeningDayToRestaurantCommand : ICommand<bool>
    {
        public AddDeviatingOpeningDayToRestaurantCommand(RestaurantId restaurantId, Date date, DeviatingOpeningDayStatus status)
        {
            RestaurantId = restaurantId;
            Date = date;
            Status = status;
        }
        
        public RestaurantId RestaurantId { get; }
        public Date Date { get; }
        public DeviatingOpeningDayStatus Status { get; }
    }
}