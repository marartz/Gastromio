using System;
using System.Collections.Generic;

namespace FoodOrderSystem.Domain.ViewModels
{
    public class DishCategoryViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<DishViewModel> Dishes { get; set; }
    }
}
