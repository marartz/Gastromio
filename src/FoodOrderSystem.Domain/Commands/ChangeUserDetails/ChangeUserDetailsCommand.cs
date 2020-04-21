using FoodOrderSystem.Domain.Model.User;

namespace FoodOrderSystem.Domain.Commands.ChangeUserDetails
{
    public class ChangeUserDetailsCommand : ICommand
    {
        public ChangeUserDetailsCommand(UserId userId, string name, Role role)
        {
            UserId = userId;
            Name = name;
            Role = role;
        }

        public UserId UserId { get; }
        public string Name { get; }
        public Role Role { get; }
    }
}
