using FoodOrderSystem.Domain.Model.User;
using FoodOrderSystem.Domain.ViewModels;

namespace FoodOrderSystem.Domain.Commands.AddUser
{
    public class AddUserCommand : ICommand<UserViewModel>
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
