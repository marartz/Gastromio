using System;

namespace FoodOrderSystem.App.Models
{
    public class DishVariantModel
    {
        public Guid VariantId { get; set; }
        
        public string Name { get; set; }
        
        public decimal Price { get; set; }
    }
}