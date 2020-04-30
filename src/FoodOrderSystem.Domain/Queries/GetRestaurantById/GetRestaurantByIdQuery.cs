using FoodOrderSystem.Domain.Model.Restaurant;
using FoodOrderSystem.Domain.ViewModels;

namespace FoodOrderSystem.Domain.Queries.GetRestaurantById
{
    public class GetRestaurantByIdQuery : IQuery<RestaurantViewModel>
    {
        public GetRestaurantByIdQuery(RestaurantId restaurantId)
        {
            RestaurantId = restaurantId;
        }

        public RestaurantId RestaurantId { get; }
    }
}
