using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOrderSystem.Persistence
{
    [Table("RestaurantCuisine")]
    public class RestaurantCuisineRow
    {
        public Guid RestaurantId { get; set; }
        public virtual RestaurantRow Restaurant { get; set; }
        public Guid CuisineId { get; set; }
        public virtual CuisineRow Cuisine { get; set; }
    }
}
