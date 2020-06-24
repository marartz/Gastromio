using System;
using System.Collections.Generic;

namespace FoodOrderSystem.Persistence.MongoDB
{
    public class CartInfoModel
    {
        public string OrderType { get; set; }
        public Guid RestaurantId { get; set; }
        public string RestaurantInfo { get; set; }
        public string RestaurantEmail { get; set; }
        public string RestaurantPhone { get; set; }
        public List<OrderedDishInfoModel> OrderedDishes { get; set; }
    }
}