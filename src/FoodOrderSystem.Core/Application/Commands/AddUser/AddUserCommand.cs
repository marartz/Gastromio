using FoodOrderSystem.Core.Application.DTOs;
using FoodOrderSystem.Core.Domain.Model.User;

namespace FoodOrderSystem.Core.Application.Commands.AddUser
{
    public class AddUserCommand : ICommand<UserDTO>
    {
        public AddUserCommand(Role role, string email, string password)
        {
            Role = role;
            Email = email;
            Password = password;
        }

        public Role Role { get; }
        public string Email { get; }
        public string Password { get; }
    }
}
