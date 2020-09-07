using System;
using FoodOrderSystem.Core.Common;

namespace FoodOrderSystem.Core.Domain.Model.User
{
    public class UserId : ValueType<Guid>
    {
        public UserId(Guid value) : base(value)
        {
        }
    }
}
