using System;

namespace FoodOrderSystem.Domain.Model.Dish
{
    public class DishId : ValueType<Guid>
    {
        public DishId(Guid value) : base(value)
        {
        }
    }
}
