using System;

namespace FoodOrderSystem.Domain.Model.Restaurant
{
    public class RestaurantId : ValueType<Guid>
    {
        public RestaurantId(Guid value) : base(value)
        {
        }
    }
}
