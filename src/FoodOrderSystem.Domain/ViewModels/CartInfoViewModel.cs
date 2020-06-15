using System;
using System.Collections.Generic;

namespace FoodOrderSystem.Domain.ViewModels
{
    public class CartInfoViewModel
    {
        public string OrderType { get; set; }
        public Guid RestaurantId { get; set; }
        public string RestaurantInfo { get; set; }
        public List<OrderedDishInfoViewModel> OrderedDishes { get; set; }
    }
}