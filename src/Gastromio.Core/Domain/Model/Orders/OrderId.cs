using System;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;

namespace Gastromio.Core.Domain.Model.Orders
{
    public class OrderId : ValueType<Guid>
    {
        public OrderId(Guid value) : base(value)
        {
            if (value == Guid.Empty)
                throw DomainException.CreateFrom(new OrderIdIsInvalidFailure());
        }
    }
}
