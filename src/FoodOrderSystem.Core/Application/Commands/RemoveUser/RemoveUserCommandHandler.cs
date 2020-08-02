using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FoodOrderSystem.Core.Application.Ports.Persistence;
using FoodOrderSystem.Core.Common;
using FoodOrderSystem.Core.Domain.Model.User;

namespace FoodOrderSystem.Core.Application.Commands.RemoveUser
{
    public class RemoveUserCommandHandler : ICommandHandler<RemoveUserCommand, bool>
    {
        private readonly IUserRepository userRepository;
        private readonly IRestaurantRepository restaurantRepository;

        public RemoveUserCommandHandler(IUserRepository userRepository, IRestaurantRepository restaurantRepository)
        {
            this.userRepository = userRepository;
            this.restaurantRepository = restaurantRepository;
        }

        public async Task<Result<bool>> HandleAsync(RemoveUserCommand command, User currentUser, CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (currentUser == null)
                return FailureResult<bool>.Unauthorized();

            if (currentUser.Role < Role.SystemAdmin)
                return FailureResult<bool>.Forbidden();

            if (command.UserId == currentUser.Id)
                return FailureResult<bool>.Create(FailureResultCode.CannotRemoveCurrentUser);

            var restaurants = await restaurantRepository.FindByUserIdAsync(command.UserId, cancellationToken);
            var restaurantList = restaurants.ToList();
            if (restaurantList.Any())
            {
                return FailureResult<bool>.Create(FailureResultCode.UserIsRestaurantAdmin,
                    string.Join(", ", restaurantList.Select(en => en.Name)));
            }

            await userRepository.RemoveAsync(command.UserId, cancellationToken);

            return SuccessResult<bool>.Create(true);
        }
    }
}
