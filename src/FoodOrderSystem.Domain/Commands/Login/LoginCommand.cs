using FoodOrderSystem.Domain.ViewModels;

namespace FoodOrderSystem.Domain.Commands.Login
{
    public class LoginCommand : ICommand<UserViewModel>
    {
        public LoginCommand(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public string Username { get; }
        public string Password { get; }
    }
}
