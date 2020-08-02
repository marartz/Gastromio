using System;
using FoodOrderSystem.Core.Common;

namespace FoodOrderSystem.Core.Domain.Model.RestaurantImage
{
    public class RestaurantImageId : ValueType<Guid>
    {
        public RestaurantImageId(Guid value) : base(value)
        {
        }
    }
}