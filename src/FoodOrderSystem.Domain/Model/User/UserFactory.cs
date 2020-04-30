using System;

namespace FoodOrderSystem.Domain.Model.User
{
    public class UserFactory : IUserFactory
    {
        public User Create(string name, Role role, string email, string password)
        {
            var userId = new UserId(Guid.NewGuid());
            var user = new User(userId, name, role, email, null, null);
            if (!string.IsNullOrEmpty(password))
                user.ChangePassword(password);
            return user;
        }
    }
}
