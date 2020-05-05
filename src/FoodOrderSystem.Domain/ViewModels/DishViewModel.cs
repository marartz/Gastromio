using System;
using System.Collections.Generic;

namespace FoodOrderSystem.Domain.ViewModels
{
    public class DishViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ProductInfo { get; set; }
        public IList<DishVariantViewModel> Variants { get; set; }
    }
}
