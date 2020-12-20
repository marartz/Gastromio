namespace Gastromio.Core.Application.Commands.RequestPasswordChange
{
    public class RequestPasswordChangeCommand : ICommand<bool>
    {
        public RequestPasswordChangeCommand(string userEmail)
        {
            UserEmail = userEmail;
        }
        
        public string UserEmail { get; }
    }
}