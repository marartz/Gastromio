namespace Gastromio.Core.Application.Commands.RequestPasswordChange
{
    public class RequestPasswordChangeCommand : ICommand
    {
        public RequestPasswordChangeCommand(string userEmail)
        {
            UserEmail = userEmail;
        }

        public string UserEmail { get; }
    }
}
