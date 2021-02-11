using System;

namespace Gastromio.Persistence.MongoDB
{
    public class DishCategoryModel
    {
        public Guid Id { get; set; }

        public Guid RestaurantId { get; set; }

        public string Name { get; set; }

        public int OrderNo { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTimeOffset UpdatedOn { get; set; }

        public Guid UpdatedBy { get; set; }
    }
}
