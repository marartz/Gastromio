using FoodOrderSystem.Domain.Model.User;
using System;

namespace FoodOrderSystem.Domain.ViewModels
{
    public class UserViewModel
    {
        public UserViewModel(Guid id, string name, string role, string email)
        {
            Id = id;
            Name = name;
            Role = role;
            Email = email;
        }

        public Guid Id { get; }

        public string Name { get; }

        public string Role { get; }

        public string Email { get; }

        public static UserViewModel FromUser(User user)
        {
            return new UserViewModel(user.Id.Value, user.Name, user.Role.ToString(), user.Email);
        }
    }
}
