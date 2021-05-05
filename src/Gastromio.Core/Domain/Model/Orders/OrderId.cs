using System;
using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Model.Orders
{
    public class OrderId : ValueType<Guid>
    {
        public OrderId(Guid value) : base(value)
        {
            if (value == Guid.Empty)
                throw new InvalidOperationException("order id is invalid");
        }
    }
}
