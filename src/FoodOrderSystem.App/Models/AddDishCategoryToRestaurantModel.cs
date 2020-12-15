using System;

namespace FoodOrderSystem.App.Models
{
    public class AddDishCategoryToRestaurantModel
    {
        public string Name { get; set; }
        
        public Guid? AfterCategoryId { get; set; }
    }
}
