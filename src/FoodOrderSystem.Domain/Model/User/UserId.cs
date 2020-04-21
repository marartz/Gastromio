using System;

namespace FoodOrderSystem.Domain.Model.User
{
    public class UserId : ValueType<Guid>
    {
        public UserId(Guid value) : base(value)
        {
        }
    }
}
