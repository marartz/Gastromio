using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOrderSystem.Persistence
{
    [Table("DishVariant")]
    public class DishVariantRow
    {
        public Guid DishId { get; set; }

        public virtual DishRow Dish { get; set; }

        public Guid VariantId { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Name { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal Price { get; set; }

        public virtual ICollection<DishVariantExtraRow> Extras { get; set; }
    }
}