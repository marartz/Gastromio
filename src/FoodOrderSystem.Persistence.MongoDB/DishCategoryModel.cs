using System;

namespace FoodOrderSystem.Persistence.MongoDB
{
    public class DishCategoryModel
    {
        public Guid Id { get; set; }

        public Guid RestaurantId { get; set; }

        public string Name { get; set; }
        
        public int OrderNo { get; set; }
    }
}