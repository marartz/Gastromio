using System;
using System.Collections.Generic;

namespace Gastromio.Persistence.MongoDB
{
    public class CartInfoModel
    {
        public string OrderType { get; set; }
        public Guid RestaurantId { get; set; }
        public string RestaurantName { get; set; }
        public string RestaurantInfo { get; set; }
        public string RestaurantEmail { get; set; }
        public string RestaurantPhone { get; set; }
        public string RestaurantMobile { get; set; }
        
        public bool RestaurantNeedsSupport { get; set; }
        public List<OrderedDishInfoModel> OrderedDishes { get; set; }
    }
}