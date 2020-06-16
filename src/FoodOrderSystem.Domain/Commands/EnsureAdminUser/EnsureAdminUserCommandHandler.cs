using FoodOrderSystem.Domain.Model;
using FoodOrderSystem.Domain.Model.User;
using System;
using System.Linq;
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

        public async Task<Result<bool>> HandleAsync(EnsureAdminUserCommand command, User currentUser, CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (currentUser == null)
                return FailureResult<bool>.Unauthorized();

            if (currentUser.Role < Role.SystemAdmin)
                return FailureResult<bool>.Forbidden();

            var systemAdminUsers = await userRepository.FindByRoleAsync(Role.SystemAdmin);
            if (systemAdminUsers.Any())
                return SuccessResult<bool>.Create(true);

            var adminUser = new User(
                new UserId(Guid.Parse("BDD00A34-F631-4BA1-94D9-C6C909475247")),
                Role.SystemAdmin,
                "admin@gastromio.de",
                null,
                null,
                DateTime.UtcNow,
                currentUser.Id,
                DateTime.UtcNow,
                currentUser.Id
            );
            adminUser.ChangePassword("admin", false, currentUser.Id);
            
            await userRepository.StoreAsync(adminUser, cancellationToken);

            return SuccessResult<bool>.Create(true);
        }
    }
}
