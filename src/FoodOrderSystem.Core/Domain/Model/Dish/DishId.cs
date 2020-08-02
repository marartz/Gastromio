using System;
using FoodOrderSystem.Core.Common;


namespace FoodOrderSystem.Core.Domain.Model.Dish
{
    public class DishId : ValueType<Guid>
    {
        public DishId(Guid value) : base(value)
        {
        }
    }
}
