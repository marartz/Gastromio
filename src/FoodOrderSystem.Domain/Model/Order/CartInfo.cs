using System.Collections.Generic;
using FoodOrderSystem.Domain.Model.Restaurant;

namespace FoodOrderSystem.Domain.Model.Order
{
    public class CartInfo
    {
        public CartInfo(
            OrderType orderType,
            RestaurantId restaurantId,
            string restaurantName,
            string restaurantInfo,
            string restaurantPhone,
            string restaurantEmail,
            IList<OrderedDishInfo> orderedDishes
        )
        {
            OrderType = orderType;
            RestaurantId = restaurantId;
            RestaurantName = restaurantName;
            RestaurantInfo = restaurantInfo;
            RestaurantPhone = restaurantPhone;
            RestaurantEmail = restaurantEmail;
            OrderedDishes = orderedDishes;
        }
        
        public OrderType OrderType { get; }
        public RestaurantId RestaurantId { get; }
        public string RestaurantName { get; }
        public string RestaurantInfo { get; }
        public string RestaurantPhone { get; }
        public string RestaurantEmail { get; }
        public IList<OrderedDishInfo> OrderedDishes { get; }
    }
}