using FoodOrderSystem.Domain.ViewModels;
using System.Collections.Generic;

namespace FoodOrderSystem.Domain.Queries.RestAdminMyRestaurants
{
    public class RestAdminMyRestaurantsQuery : IQuery<ICollection<RestaurantViewModel>>
    {
        public RestAdminMyRestaurantsQuery()
        {
        }
    }
}
