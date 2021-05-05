using System;
using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Model.Restaurants
{
    public class DishVariantId : ValueType<Guid>
    {
        public DishVariantId(Guid value) : base(value)
        {
            if (value == Guid.Empty)
                throw new InvalidOperationException("dish variant id is invalid");
        }
    }
}
