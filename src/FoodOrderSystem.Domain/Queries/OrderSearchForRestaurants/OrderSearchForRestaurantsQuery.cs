using FoodOrderSystem.Domain.ViewModels;
using System.Collections.Generic;

namespace FoodOrderSystem.Domain.Queries.OrderSearchForRestaurants
{
    public class OrderSearchForRestaurantsQuery : IQuery<ICollection<RestaurantViewModel>>
    {
        public OrderSearchForRestaurantsQuery(string searchPhrase)
        {
            SearchPhrase = searchPhrase;
        }

        public string SearchPhrase { get; }
    }
}
