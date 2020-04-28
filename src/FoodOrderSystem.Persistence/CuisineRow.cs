using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOrderSystem.Persistence
{
    [Table("Cuisine")]
    public class CuisineRow
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public byte[] Image { get; set; }
        public virtual ICollection<RestaurantCuisineRow> RestaurantCuisines { get; set; } = new List<RestaurantCuisineRow>();
    }
}
