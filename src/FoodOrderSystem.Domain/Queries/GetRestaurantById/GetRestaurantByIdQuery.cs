using FoodOrderSystem.Domain.Model.Restaurant;

namespace FoodOrderSystem.Domain.Queries.GetRestaurantById
{
    public class GetRestaurantByIdQuery : IQuery
    {
        public GetRestaurantByIdQuery(RestaurantId restaurantId)
        {
            RestaurantId = restaurantId;
        }

        public RestaurantId RestaurantId { get; }
    }
}
