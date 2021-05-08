using System;
using System.Threading;
using System.Threading.Tasks;
using Gastromio.Core.Application.Ports.Persistence;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;
using Gastromio.Core.Domain.Model.Users;

namespace Gastromio.Core.Application.Commands.AddOrChangeDishOfRestaurant
{
    public class AddOrChangeDishOfRestaurantCommandHandler : ICommandHandler<AddOrChangeDishOfRestaurantCommand, Guid>
    {
        private readonly IRestaurantRepository restaurantRepository;

        public AddOrChangeDishOfRestaurantCommandHandler(IRestaurantRepository restaurantRepository)
        {
            this.restaurantRepository = restaurantRepository;
        }

        public async Task<Result<Guid>> HandleAsync(AddOrChangeDishOfRestaurantCommand command, User currentUser,
            CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (currentUser == null)
                return FailureResult<Guid>.Unauthorized();

            if (currentUser.Role < Role.RestaurantAdmin)
                return FailureResult<Guid>.Forbidden();

            var restaurant =
                await restaurantRepository.FindByRestaurantIdAsync(command.RestaurantId, cancellationToken);
            if (restaurant == null)
                throw DomainException.CreateFrom(new RestaurantDoesNotExistFailure());

            if (currentUser.Role == Role.RestaurantAdmin && !restaurant.HasAdministrator(currentUser.Id))
                return FailureResult<Guid>.Forbidden();

            var dish = restaurant.AddOrChangeDish(
                command.DishCategoryId,
                command.DishId,
                command.Name,
                command.Description,
                command.ProductInfo,
                command.OrderNo,
                command.Variants,
                currentUser.Id
            );

            await restaurantRepository.StoreAsync(restaurant, cancellationToken);

            return SuccessResult<Guid>.Create(dish.Id.Value);
        }
    }
}
