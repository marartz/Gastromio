using FoodOrderSystem.Domain.Model.Restaurant;

namespace FoodOrderSystem.Domain.Queries.GetCategoriesOfRestaurant
{
    public class GetCategoriesOfRestaurantQuery : IQuery
    {
        public GetCategoriesOfRestaurantQuery(RestaurantId restaurantId)
        {
            RestaurantId = restaurantId;
        }

        public RestaurantId RestaurantId { get; }
    }
}
