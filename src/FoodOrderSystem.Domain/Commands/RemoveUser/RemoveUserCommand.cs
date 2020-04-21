using FoodOrderSystem.Domain.Model.User;

namespace FoodOrderSystem.Domain.Commands.RemoveUser
{
    public class RemoveUserCommand : ICommand
    {
        public RemoveUserCommand(UserId userId)
        {
            UserId = userId;
        }

        public UserId UserId { get; }
    }
}
