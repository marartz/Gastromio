using FoodOrderSystem.Domain.ViewModels;

namespace FoodOrderSystem.Domain.Commands.Login
{
    public class LoginCommand : ICommand<UserViewModel>
    {
        public LoginCommand(string email, string password)
        {
            Email = email;
            Password = password;
        }

        public string Email { get; }
        public string Password { get; }
    }
}
