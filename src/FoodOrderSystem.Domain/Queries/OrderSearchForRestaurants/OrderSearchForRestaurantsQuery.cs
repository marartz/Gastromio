using FoodOrderSystem.Domain.ViewModels;
using System.Collections.Generic;
using FoodOrderSystem.Domain.Model.Order;

namespace FoodOrderSystem.Domain.Queries.OrderSearchForRestaurants
{
    public class OrderSearchForRestaurantsQuery : IQuery<ICollection<RestaurantViewModel>>
    {
        public OrderSearchForRestaurantsQuery(string searchPhrase, OrderType? orderType)
        {
            SearchPhrase = searchPhrase;
            OrderType = orderType;
        }

        public string SearchPhrase { get; }
        public OrderType? OrderType { get; }
    }
}
