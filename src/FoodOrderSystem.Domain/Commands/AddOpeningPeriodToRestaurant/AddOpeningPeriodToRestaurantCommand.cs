using FoodOrderSystem.Domain.Model.Restaurant;
using System;

namespace FoodOrderSystem.Domain.Commands.AddOpeningPeriodToRestaurant
{
    public class AddOpeningPeriodToRestaurantCommand : ICommand<bool>
    {
        public AddOpeningPeriodToRestaurantCommand(RestaurantId restaurantId, OpeningPeriod openingPeriod)
        {
            RestaurantId = restaurantId;
            OpeningPeriod = openingPeriod;
        }

        public RestaurantId RestaurantId { get; }
        
        public OpeningPeriod OpeningPeriod { get; }
    }
}
