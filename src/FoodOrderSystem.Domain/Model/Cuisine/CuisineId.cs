using System;

namespace FoodOrderSystem.Domain.Model.Cuisine
{
    public class CuisineId : ValueType<Guid>
    {
        public CuisineId(Guid value) : base(value)
        {
        }
    }
}
