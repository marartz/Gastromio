using System;
using FoodOrderSystem.Core.Common;

namespace FoodOrderSystem.Core.Domain.Model.Restaurant
{
    public class RestaurantId : ValueType<Guid>
    {
        public RestaurantId(Guid value) : base(value)
        {
        }
    }
}
