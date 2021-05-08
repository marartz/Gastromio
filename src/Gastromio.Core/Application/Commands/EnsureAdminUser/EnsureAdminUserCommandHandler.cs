using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gastromio.Core.Application.Ports.Persistence;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;
using Gastromio.Core.Domain.Model.Users;

namespace Gastromio.Core.Application.Commands.EnsureAdminUser
{
    public class EnsureAdminUserCommandHandler : ICommandHandler<EnsureAdminUserCommand>
    {
        private readonly IUserRepository userRepository;

        public EnsureAdminUserCommandHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task HandleAsync(EnsureAdminUserCommand command, User currentUser,
            CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (currentUser == null)
                throw DomainException.CreateFrom(new SessionExpiredFailure());

            if (currentUser.Role < Role.SystemAdmin)
                throw DomainException.CreateFrom(new ForbiddenFailure());

            var systemAdminUsers = await userRepository.FindByRoleAsync(Role.SystemAdmin, cancellationToken);
            if (systemAdminUsers.Any())
                return;

            var adminUser = new User(
                new UserId(Guid.Parse("BDD00A34-F631-4BA1-94D9-C6C909475247")),
                Role.SystemAdmin,
                "admin@gastromio.de",
                null,
                null,
                null,
                null,
                DateTimeOffset.UtcNow,
                currentUser.Id,
                DateTimeOffset.UtcNow,
                currentUser.Id
            );
            adminUser.ChangePassword("admin", false, currentUser.Id);

            await userRepository.StoreAsync(adminUser, cancellationToken);
        }
    }
}
