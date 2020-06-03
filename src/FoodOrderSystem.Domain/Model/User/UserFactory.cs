using System;

namespace FoodOrderSystem.Domain.Model.User
{
    public class UserFactory : IUserFactory
    {
        public Result<User> Create(Role role, string email, string password)
        {
            var user = new User(new UserId(Guid.NewGuid()));

            var tempResult = user.ChangeDetails(role, email);
            if (tempResult.IsFailure)
                return tempResult.Cast<User>();

            if (!string.IsNullOrEmpty(password))
            {
                tempResult = user.ChangePassword(password);
                if (tempResult.IsFailure)
                    return tempResult.Cast<User>();
            }

            return SuccessResult<User>.Create(user);
        }
    }
}
