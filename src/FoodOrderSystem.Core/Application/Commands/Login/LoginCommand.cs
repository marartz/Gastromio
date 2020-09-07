using FoodOrderSystem.Core.Application.DTOs;

namespace FoodOrderSystem.Core.Application.Commands.Login
{
    public class LoginCommand : ICommand<UserDTO>
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
