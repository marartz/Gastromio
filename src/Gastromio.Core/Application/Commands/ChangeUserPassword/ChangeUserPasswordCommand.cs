using Gastromio.Core.Domain.Model.Users;

namespace Gastromio.Core.Application.Commands.ChangeUserPassword
{
    public class ChangeUserPasswordCommand : ICommand
    {
        public ChangeUserPasswordCommand(UserId userId, string password)
        {
            UserId = userId;
            Password = password;
        }

        public UserId UserId { get; }
        public string Password { get; }
    }
}
