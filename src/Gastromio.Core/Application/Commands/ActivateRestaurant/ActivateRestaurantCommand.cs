﻿using Gastromio.Core.Domain.Model.Restaurants;

namespace Gastromio.Core.Application.Commands.ActivateRestaurant
{
    public class ActivateRestaurantCommand : ICommand
    {
        public ActivateRestaurantCommand(RestaurantId restaurantId)
        {
            RestaurantId = restaurantId;
        }

        public RestaurantId RestaurantId { get; }
    }
}
