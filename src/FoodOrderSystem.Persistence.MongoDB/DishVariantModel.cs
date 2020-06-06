using System;

namespace FoodOrderSystem.Persistence.MongoDB
{
    public class DishVariantModel
    {
        public Guid VariantId { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }    }
}