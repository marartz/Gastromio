using System;
using Gastromio.Core.Common;


namespace Gastromio.Core.Domain.Model.Dish
{
    public class DishId : ValueType<Guid>
    {
        public DishId(Guid value) : base(value)
        {
        }
    }
}
