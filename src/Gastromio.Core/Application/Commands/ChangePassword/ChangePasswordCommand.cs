namespace Gastromio.Core.Application.Commands.ChangePassword
{
    public class ChangePasswordCommand : ICommand
    {
        public ChangePasswordCommand(string password)
        {
            Password = password;
        }

        public string Password { get; }
    }
}
