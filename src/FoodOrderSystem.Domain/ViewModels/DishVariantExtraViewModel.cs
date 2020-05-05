using System;

namespace FoodOrderSystem.Domain.ViewModels
{
    public class DishVariantExtraViewModel
    {
        public Guid ExtraId { get; set; }
        public string Name { get; set; }
        public string ProductInfo { get; set; }
        public decimal Price { get; set; }
    }
}
