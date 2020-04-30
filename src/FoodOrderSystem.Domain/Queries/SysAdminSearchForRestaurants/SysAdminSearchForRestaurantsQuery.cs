using FoodOrderSystem.Domain.ViewModels;
using System.Collections.Generic;

namespace FoodOrderSystem.Domain.Queries.SysAdminSearchForRestaurants
{
    public class SysAdminSearchForRestaurantsQuery : IQuery<ICollection<RestaurantViewModel>>
    {
        public SysAdminSearchForRestaurantsQuery(string searchPhrase)
        {
            SearchPhrase = searchPhrase;
        }

        public string SearchPhrase { get; }
    }
}
