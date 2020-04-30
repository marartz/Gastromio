using FoodOrderSystem.Domain.ViewModels;
using System.Collections.Generic;

namespace FoodOrderSystem.Domain.Queries.GetAllRestaurants
{
    public class GetAllRestaurantsQuery : IQuery<ICollection<RestaurantViewModel>>
    {
    }
}
