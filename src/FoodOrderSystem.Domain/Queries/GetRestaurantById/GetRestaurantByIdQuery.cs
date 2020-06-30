using FoodOrderSystem.Domain.Model.Restaurant;
using FoodOrderSystem.Domain.ViewModels;

namespace FoodOrderSystem.Domain.Queries.GetRestaurantById
{
    public class GetRestaurantByIdQuery : IQuery<RestaurantViewModel>
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
