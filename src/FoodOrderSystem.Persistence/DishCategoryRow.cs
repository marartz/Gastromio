using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOrderSystem.Persistence
{
    [Table("DishCategory")]
    public class DishCategoryRow
    {
        public Guid Id { get; set; }

        public Guid RestaurantId { get; set; }

        public virtual RestaurantRow Restaurant { get; set; }

        public string Name { get; set; }
        
        public int OrderNo { get; set; }

        public virtual ICollection<DishRow> Dishes { get; set; } = new List<DishRow>();
    }
}
