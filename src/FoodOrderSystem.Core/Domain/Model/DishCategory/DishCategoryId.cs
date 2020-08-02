using System;
using FoodOrderSystem.Core.Common;

namespace FoodOrderSystem.Core.Domain.Model.DishCategory
{
    public class DishCategoryId : ValueType<Guid>
    {
        public DishCategoryId(Guid value) : base(value)
        {
        }
    }
}
