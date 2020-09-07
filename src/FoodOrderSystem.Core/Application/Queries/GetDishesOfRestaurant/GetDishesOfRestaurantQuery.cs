using System.Collections.Generic;
using FoodOrderSystem.Core.Application.DTOs;
using FoodOrderSystem.Core.Domain.Model.Restaurant;

namespace FoodOrderSystem.Core.Application.Queries.GetDishesOfRestaurant
{
    public class GetDishesOfRestaurantQuery : IQuery<ICollection<DishCategoryDTO>>
    {
        public GetDishesOfRestaurantQuery(string restaurantId)
        {
            RestaurantId = restaurantId;
        }

        public string RestaurantId { get; }
    }
}
