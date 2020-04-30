using FoodOrderSystem.Domain.Model.User;
using FoodOrderSystem.Domain.ViewModels;

namespace FoodOrderSystem.Domain.Commands.AddUser
{
    public class AddUserCommand : ICommand<UserViewModel>
    {
        public AddUserCommand(string name, Role role, string email, string password)
        {
            Name = name;
            Role = role;
            Email = email;
            Password = password;
        }

        public string Name { get; }
        public Role Role { get; }
        public string Email { get; }
        public string Password { get; }
    }
}
