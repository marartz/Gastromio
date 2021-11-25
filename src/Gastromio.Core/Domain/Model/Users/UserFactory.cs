using System;

namespace Gastromio.Core.Domain.Model.Users
{
    public class UserFactory : IUserFactory
    {
        public User Create(
            Role role,
            string email,
            string password,
            bool checkPasswordPolicy,
            UserId createdBy
        )
        {
            var user = new User(
                new UserId(Guid.NewGuid()),
                role,
                email,
                null,
                null,
                null,
                null,
                DateTimeOffset.UtcNow,
                createdBy,
                DateTimeOffset.UtcNow,
                createdBy
            );

            if (!string.IsNullOrEmpty(password))
            {
                user.ChangePassword(password, checkPasswordPolicy, createdBy);
            }

            return user;
        }
    }
}
