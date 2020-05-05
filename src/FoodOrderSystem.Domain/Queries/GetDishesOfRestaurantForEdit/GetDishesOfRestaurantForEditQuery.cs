using FoodOrderSystem.Domain.Model.Restaurant;
using FoodOrderSystem.Domain.ViewModels;
using System.Collections.Generic;

namespace FoodOrderSystem.Domain.Queries.GetDishesOfRestaurantForEdit
{
    public class GetDishesOfRestaurantForEditQuery : IQuery<ICollection<DishCategoryViewModel>>
    {
        public GetDishesOfRestaurantForEditQuery(RestaurantId restaurantId)
        {
            RestaurantId = restaurantId;
        }

        public RestaurantId RestaurantId { get; }
    }
}
