using System;
using System.Collections.Generic;

namespace Gastromio.Persistence.MongoDB
{
    public class DishCategoryModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int OrderNo { get; set; }

        public bool? Enabled { get; set; }

        public IList<DishModel> Dishes { get; set; }
    }
}
