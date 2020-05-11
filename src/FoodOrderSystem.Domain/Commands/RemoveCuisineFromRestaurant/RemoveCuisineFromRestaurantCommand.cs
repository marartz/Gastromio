using FoodOrderSystem.Domain.Model.Cuisine;
using FoodOrderSystem.Domain.Model.Restaurant;

namespace FoodOrderSystem.Domain.Commands.RemoveCuisineFromRestaurant
{
    public class RemoveCuisineFromRestaurantCommand : ICommand<bool>
    {
        public RemoveCuisineFromRestaurantCommand(RestaurantId restaurantId, CuisineId cuisineId)
        {
            RestaurantId = restaurantId;
            CuisineId = cuisineId;
        }

        public RestaurantId RestaurantId { get; }
        public CuisineId CuisineId { get; }
    }
}
