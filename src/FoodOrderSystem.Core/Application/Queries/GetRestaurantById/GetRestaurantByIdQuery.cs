using FoodOrderSystem.Core.Application.DTOs;
using FoodOrderSystem.Core.Domain.Model.Restaurant;

namespace FoodOrderSystem.Core.Application.Queries.GetRestaurantById
{
    public class GetRestaurantByIdQuery : IQuery<RestaurantDTO>
    {
        public GetRestaurantByIdQuery(RestaurantId restaurantId, bool onlyActiveRestaurants)
        {
            RestaurantId = restaurantId;
            OnlyActiveRestaurants = onlyActiveRestaurants;
        }

        public RestaurantId RestaurantId { get; }
        
        public bool OnlyActiveRestaurants { get; }
    }
}
