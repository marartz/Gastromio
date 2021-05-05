using System;
using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Model.Restaurants
{
    public class DishId : ValueType<Guid>
    {
        public DishId(Guid value) : base(value)
        {
            if (value == Guid.Empty)
                throw new InvalidOperationException("dish id is invalid");
        }
    }
}
