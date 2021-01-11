using System.Collections.Generic;
using Gastromio.Core.Application.DTOs;
using Gastromio.Core.Domain.Model.Restaurant;

namespace Gastromio.Core.Application.Queries.GetDishesOfRestaurant
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
