using FoodOrderSystem.Domain.Model.Cuisine;
using FoodOrderSystem.Domain.Model.Restaurant;

namespace FoodOrderSystem.Domain.Commands.AddCuisineToRestaurant
{
    public class AddCuisineToRestaurantCommand : ICommand<bool>
    {
        public AddCuisineToRestaurantCommand(RestaurantId restaurantId, CuisineId cuisineId)
        {
            RestaurantId = restaurantId;
            CuisineId = cuisineId;
        }

        public RestaurantId RestaurantId { get; }
        public CuisineId CuisineId { get; }
    }
}
