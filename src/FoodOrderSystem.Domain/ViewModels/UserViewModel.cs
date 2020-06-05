using FoodOrderSystem.Domain.Model.User;
using System;

namespace FoodOrderSystem.Domain.ViewModels
{
    public class UserViewModel
    {
        public Guid Id { get; set; }

        public string Role { get; set; }

        public string Email { get; set; }

        public static UserViewModel FromUser(User user)
        {
            return new UserViewModel
            {
                Id = user.Id.Value,
                Role = user.Role.ToString(),
                Email = user.Email
            };
        }
    }
}
