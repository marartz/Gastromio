using System;

namespace FoodOrderSystem.Domain.Model.User
{
    public class UserFactory : IUserFactory
    {
        public Result<User> Create(
            Role role,
            string email,
            string password,
            UserId createdBy
        )
        {
            var user = new User(
                new UserId(Guid.NewGuid()),
                DateTime.UtcNow,
                createdBy,
                DateTime.UtcNow,
                createdBy
            );

            var tempResult = user.ChangeDetails(role, email, createdBy);
            if (tempResult.IsFailure)
                return tempResult.Cast<User>();

            if (!string.IsNullOrEmpty(password))
            {
                tempResult = user.ChangePassword(password, createdBy);
                if (tempResult.IsFailure)
                    return tempResult.Cast<User>();
            }

            return SuccessResult<User>.Create(user);
        }
    }
}
