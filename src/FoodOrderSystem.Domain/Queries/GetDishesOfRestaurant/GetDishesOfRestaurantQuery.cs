using FoodOrderSystem.Domain.Model.Restaurant;
using FoodOrderSystem.Domain.ViewModels;
using System.Collections.Generic;

namespace FoodOrderSystem.Domain.Queries.GetDishesOfRestaurant
{
    public class GetDishesOfRestaurantQuery : IQuery<ICollection<DishCategoryViewModel>>
    {
        public GetDishesOfRestaurantQuery(RestaurantId restaurantId)
        {
            RestaurantId = restaurantId;
        }

        public RestaurantId RestaurantId { get; }
    }
}
