using FoodOrderSystem.Domain.Model;
using FoodOrderSystem.Domain.Model.DishCategory;
using FoodOrderSystem.Domain.Model.Restaurant;
using FoodOrderSystem.Domain.Model.User;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Domain.Commands.AddDishCategoryToRestaurant
{
    public class AddDishCategoryToRestaurantCommandHandler : ICommandHandler<AddDishCategoryToRestaurantCommand, Guid>
    {
        private readonly IRestaurantRepository restaurantRepository;
        private readonly IDishCategoryRepository dishCategoryRepository;
        private readonly IDishCategoryFactory dishCategoryFactory;

        public AddDishCategoryToRestaurantCommandHandler(IRestaurantRepository restaurantRepository,
            IDishCategoryRepository dishCategoryRepository, IDishCategoryFactory dishCategoryFactory)
        {
            this.restaurantRepository = restaurantRepository;
            this.dishCategoryRepository = dishCategoryRepository;
            this.dishCategoryFactory = dishCategoryFactory;
        }

        public async Task<Result<Guid>> HandleAsync(AddDishCategoryToRestaurantCommand command, User currentUser,
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
                return FailureResult<Guid>.Create(FailureResultCode.RestaurantDoesNotExist);

            if (currentUser.Role == Role.RestaurantAdmin && !restaurant.HasAdministrator(currentUser.Id))
                return FailureResult<Guid>.Forbidden();

            var createResult = dishCategoryFactory.Create(command.RestaurantId, command.Name);
            if (createResult.IsFailure)
                return createResult.Cast<Guid>();

            var dishCategory = createResult.Value;
            
            await dishCategoryRepository.StoreAsync(dishCategory, cancellationToken);

            return SuccessResult<Guid>.Create(dishCategory.Id.Value);
        }
    }
}