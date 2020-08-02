using System;
using FoodOrderSystem.Core.Common;

namespace FoodOrderSystem.Core.Domain.Model.Order
{
    public class OrderId : ValueType<Guid>
    {
        public OrderId(Guid value) : base(value)
        {
        }
    }
}