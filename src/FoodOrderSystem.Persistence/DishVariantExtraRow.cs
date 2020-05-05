using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOrderSystem.Persistence
{
    [Table("DishVariantExtra")]
    public class DishVariantExtraRow
    {
        public Guid DishId { get; set; }

        public virtual DishRow Dish { get; set; }

        public Guid VariantId { get; set; }

        public virtual DishVariantRow Variant { get; set; }

        public Guid ExtraId { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Name { get; set; }

        public string ProductInfo { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal Price { get; set; }
    }
}