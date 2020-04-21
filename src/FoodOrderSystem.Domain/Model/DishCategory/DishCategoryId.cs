using System;

namespace FoodOrderSystem.Domain.Model.DishCategory
{
    public class DishCategoryId : ValueType<Guid>
    {
        public DishCategoryId(Guid value) : base(value)
        {
        }
    }
}
