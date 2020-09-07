using System.Collections.Generic;
using FoodOrderSystem.Core.Application.DTOs;

namespace FoodOrderSystem.Core.Application.Queries.GetAllRestaurants
{
    public class GetAllRestaurantsQuery : IQuery<ICollection<RestaurantDTO>>
    {
    }
}
