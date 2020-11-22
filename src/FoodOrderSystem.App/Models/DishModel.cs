using System;
using System.Collections.Generic;

namespace FoodOrderSystem.App.Models
{
    public class DishModel
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public string ProductInfo { get; set; }
        
        public int OrderNo { get; set; }
        
        public List<DishVariantModel> Variants { get; set; }
    }
}