namespace FoodOrderSystem.Domain.Commands.Login
{
    public class LoginCommand : ICommand
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
