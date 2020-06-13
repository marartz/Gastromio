using System;

namespace FoodOrderSystem.Domain.Model.Order
{
    public class OrderId : ValueType<Guid>
    {
        public OrderId(Guid value) : base(value)
        {
        }
    }
}