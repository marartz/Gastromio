using Gastromio.Core.Domain.Model.Cuisine;
using Gastromio.Core.Domain.Model.Restaurant;

namespace Gastromio.Core.Application.Commands.RemoveCuisineFromRestaurant
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
