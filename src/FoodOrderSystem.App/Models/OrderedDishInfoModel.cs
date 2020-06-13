using System;

namespace FoodOrderSystem.App.Models
{
    public class OrderedDishInfoModel
    {
        public Guid ItemId { get; set; }
        public Guid DishId { get; set; }
        public Guid VariantId { get; set; }
        public int Count { get; set; }
        public string Remarks { get; set; }

    }
}