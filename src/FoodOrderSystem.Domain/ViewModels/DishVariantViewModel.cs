using System;
using System.Collections.Generic;

namespace FoodOrderSystem.Domain.ViewModels
{
    public class DishVariantViewModel
    {
        public Guid VariantId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public List<DishVariantExtraViewModel> Extras { get; set; }
    }
}
