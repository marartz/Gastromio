using System.Collections.Generic;
using FoodOrderSystem.Domain.Model.Restaurant;

namespace FoodOrderSystem.Domain.Model.Order
{
    public class CartInfo
    {
        public CartInfo(
            OrderType orderType,
            RestaurantId restaurantId,
            string restaurantInfo,
            IList<OrderedDishInfo> orderedDishes
        )
        {
            OrderType = orderType;
            RestaurantId = restaurantId;
            RestaurantInfo = restaurantInfo;
            OrderedDishes = orderedDishes;
        }
        
        public OrderType OrderType { get; }
        public RestaurantId RestaurantId { get; }
        public string RestaurantInfo { get; }
        public IList<OrderedDishInfo> OrderedDishes { get; }
    }
}