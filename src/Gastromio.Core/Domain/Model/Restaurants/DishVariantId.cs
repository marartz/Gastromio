using System;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;

namespace Gastromio.Core.Domain.Model.Restaurants
{
    public class DishVariantId : ValueType<Guid>
    {
        public DishVariantId(Guid value) : base(value)
        {
            if (value == Guid.Empty)
                throw DomainException.CreateFrom(new DishVariantIdIsInvalidFailure());
        }
    }
}
