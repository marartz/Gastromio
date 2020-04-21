using FoodOrderSystem.Domain.Model.User;

namespace FoodOrderSystem.Domain.Commands.AddUser
{
    public class AddUserCommand : ICommand
    {
        public AddUserCommand(string name, Role role, string password)
        {
            Name = name;
            Role = role;
            Password = password;
        }

        public string Name { get; }
        public Role Role { get; }
        public string Password { get; }
    }
}
