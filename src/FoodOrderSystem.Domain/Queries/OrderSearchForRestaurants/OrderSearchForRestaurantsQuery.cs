using System;
using FoodOrderSystem.Domain.ViewModels;
using System.Collections.Generic;
using FoodOrderSystem.Domain.Model.Cuisine;
using FoodOrderSystem.Domain.Model.Order;

namespace FoodOrderSystem.Domain.Queries.OrderSearchForRestaurants
{
    public class OrderSearchForRestaurantsQuery : IQuery<ICollection<RestaurantViewModel>>
    {
        public OrderSearchForRestaurantsQuery(string searchPhrase, OrderType? orderType, CuisineId cuisineId, DateTime? openingHour)
        {
            SearchPhrase = searchPhrase;
            OrderType = orderType;
            CuisineId = cuisineId;
            OpeningHour = openingHour;
        }

        public string SearchPhrase { get; }
        public OrderType? OrderType { get; }
        public CuisineId CuisineId { get; }
        public DateTime? OpeningHour { get; }
    }
}