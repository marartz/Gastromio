using System;

namespace FoodOrderSystem.Domain.Model.RestaurantImage
{
    public class RestaurantImageId : ValueType<Guid>
    {
        public RestaurantImageId(Guid value) : base(value)
        {
        }
    }
}