using System;

namespace FoodOrderSystem.Persistence
{
    public class RestaurantCuisineRow
    {
        public Guid RestaurantId { get; set; }
        public virtual RestaurantRow Restaurant { get; set; }
        public Guid CuisineId { get; set; }
        public virtual CuisineRow Cuisine { get; set; }
    }
}
