using FoodOrderSystem.Core.Application.DTOs;
using FoodOrderSystem.Core.Domain.Model.Restaurant;

namespace FoodOrderSystem.Core.Application.Queries.GetRestaurantById
{
    public class GetRestaurantByIdQuery : IQuery<RestaurantDTO>
    {
        public GetRestaurantByIdQuery(string restaurantId, bool onlyActiveRestaurants)
        {
            RestaurantId = restaurantId;
            OnlyActiveRestaurants = onlyActiveRestaurants;
        }

        public string RestaurantId { get; }
        
        public bool OnlyActiveRestaurants { get; }
    }
}
