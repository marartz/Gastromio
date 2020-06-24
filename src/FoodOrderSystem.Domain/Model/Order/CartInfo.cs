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
            string restaurantPhone,
            string restaurantEmail,
            IList<OrderedDishInfo> orderedDishes
        )
        {
            OrderType = orderType;
            RestaurantId = restaurantId;
            RestaurantInfo = restaurantInfo;
            RestaurantPhone = restaurantPhone;
            RestaurantEmail = restaurantEmail;
            OrderedDishes = orderedDishes;
        }
        
        public OrderType OrderType { get; }
        public RestaurantId RestaurantId { get; }
        public string RestaurantInfo { get; }
        public string RestaurantPhone { get; }
        public string RestaurantEmail { get; }
        public IList<OrderedDishInfo> OrderedDishes { get; }
    }
}