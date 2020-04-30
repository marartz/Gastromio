using FoodOrderSystem.Domain.Model.User;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Domain.Commands.EnsureAdminUser
{
    public class EnsureAdminUserCommandHandler : ICommandHandler<EnsureAdminUserCommand, bool>
    {
        private readonly IUserFactory userFactory;
        private readonly IUserRepository userRepository;

        public EnsureAdminUserCommandHandler(IUserFactory userFactory, IUserRepository userRepository)
        {
            this.userFactory = userFactory;
            this.userRepository = userRepository;
        }

        public async Task<CommandResult<bool>> HandleAsync(EnsureAdminUserCommand command, User currentUser, CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (currentUser == null)
                return new UnauthorizedCommandResult<bool>();

            if (currentUser.Role < Role.SystemAdmin)
                return new ForbiddenCommandResult<bool>();

            var systemAdminUsers = await userRepository.FindByRoleAsync(Role.SystemAdmin);
            if (systemAdminUsers.Count != 0)
                return new SuccessCommandResult<bool>(true);

            var adminUser = new User(new UserId(Guid.Parse("BDD00A34-F631-4BA1-94D9-C6C909475247")), "admin", Role.SystemAdmin, "info@boh-liefert.de", null, null);
            adminUser.ChangePassword("admin");
            await userRepository.StoreAsync(adminUser, cancellationToken);

            return new SuccessCommandResult<bool>(true);
        }
    }
}
