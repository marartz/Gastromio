using System;

namespace Gastromio.Persistence.MongoDB
{
    public class DishVariantModel
    {
        public Guid VariantId { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }    }
}