using System;
using System.Collections.Generic;

namespace Gastromio.Persistence.MongoDB
{
    public class DishModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ProductInfo { get; set; }

        public int OrderNo { get; set; }

        public IList<DishVariantModel> Variants { get; set; }
    }
}
