using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOrderSystem.Persistence
{
    [Table("Dish")]
    public class DishRow
    {
        [Key]
        public Guid Id { get; set; }

        public Guid RestaurantId { get; set; }

        public virtual RestaurantRow Restaurant { get; set; }

        public Guid CategoryId { get; set; }

        public virtual DishCategoryRow Category { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ProductInfo { get; set; }

        public virtual ICollection<DishVariantRow> Variants { get; set; }
    }
}
