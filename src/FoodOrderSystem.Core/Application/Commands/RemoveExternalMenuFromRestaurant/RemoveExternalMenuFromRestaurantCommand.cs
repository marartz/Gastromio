using System;
using FoodOrderSystem.Core.Domain.Model.Restaurant;

namespace FoodOrderSystem.Core.Application.Commands.RemoveExternalMenuFromRestaurant
{
    public class RemoveExternalMenuFromRestaurantCommand : ICommand<bool>
    {
        public RemoveExternalMenuFromRestaurantCommand(RestaurantId restaurantId, Guid externalMenuId)
        {
            RestaurantId = restaurantId;
            ExternalMenuId = externalMenuId;
        }
        
        public RestaurantId RestaurantId { get; }

        public Guid ExternalMenuId { get; }
    }
}