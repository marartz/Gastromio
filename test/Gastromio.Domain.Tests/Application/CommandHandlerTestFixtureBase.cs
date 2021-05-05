using Gastromio.Core.Application.Commands;
using Gastromio.Core.Domain.Model.Users;
using Gastromio.Domain.TestKit.Domain.Model.Users;

namespace Gastromio.Domain.Tests.Application
{
    public abstract class CommandHandlerTestFixtureBase<THandler, TCommand, TResult>
        where THandler : ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult>
    {
        protected CommandHandlerTestFixtureBase(Role? minimumRole)
        {
            MinimumRole = minimumRole;

            if (!MinimumRole.HasValue)
                return;

            var email = "user@gasromio.de";

            UserId = new UserIdBuilder()
                .Create();

            UserWithCustomerRole = new UserBuilder()
                .WithId(UserId)
                .WithRole(Role.Customer)
                .WithEmail(email)
                .Create();

            UserWithRestaurantAdminRole = new UserBuilder()
                .WithId(UserId)
                .WithRole(Role.RestaurantAdmin)
                .WithEmail(email)
                .Create();

            UserWithSystemAdminRole = new UserBuilder()
                .WithId(UserId)
                .WithRole(Role.SystemAdmin)
                .WithEmail(email)
                .Create();

            UserWithMinimumRole = new UserBuilder()
                .WithId(UserId)
                .WithRole(MinimumRole.Value)
                .WithEmail(email)
                .Create();
        }

        public Role? MinimumRole { get; }

        public UserId UserId { get; }

        public User UserWithCustomerRole { get; }

        public User UserWithRestaurantAdminRole { get; }

        public User UserWithSystemAdminRole { get; }

        public User UserWithMinimumRole { get; }

        public abstract THandler CreateTestObject();

        public abstract TCommand CreateSuccessfulCommand();

        public abstract void SetupForSuccessfulCommandExecution(Role? role);
    }
}
