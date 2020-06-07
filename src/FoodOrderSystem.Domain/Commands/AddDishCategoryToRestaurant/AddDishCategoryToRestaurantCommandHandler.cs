using FoodOrderSystem.Domain.Model;
using FoodOrderSystem.Domain.Model.DishCategory;
using FoodOrderSystem.Domain.Model.Restaurant;
using FoodOrderSystem.Domain.Model.User;
using System;
using System.Linq;
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

            var curCategories =
                (await dishCategoryRepository.FindByRestaurantIdAsync(command.RestaurantId, cancellationToken))
                .OrderBy(en => en.OrderNo).ToList();

            if (curCategories.Any(en => string.Equals(en.Name, command.Name)))
                return FailureResult<Guid>.Create(FailureResultCode.DishCategoryAlreadyExists);

            var pos = curCategories.FindIndex(en => en.Id == command.AfterCategoryId);

            for (var i = 0; i <= pos; i++)
            {
                var tempResult = curCategories[i].ChangeOrderNo(i);
                if (tempResult.IsFailure)
                    return tempResult.Cast<Guid>();
                await dishCategoryRepository.StoreAsync(curCategories[i], cancellationToken);
            }

            for (var i = pos + 1; i < curCategories.Count; i++)
            {
                var tempResult = curCategories[i].ChangeOrderNo(i + 1);
                if (tempResult.IsFailure)
                    return tempResult.Cast<Guid>();
                await dishCategoryRepository.StoreAsync(curCategories[i], cancellationToken);
            }

            var createResult = dishCategoryFactory.Create(command.RestaurantId, command.Name, pos + 1);
            if (createResult.IsFailure)
                return createResult.Cast<Guid>();
            var dishCategory = createResult.Value;
            
            await dishCategoryRepository.StoreAsync(dishCategory, cancellationToken);

            return SuccessResult<Guid>.Create(dishCategory.Id.Value);
        }
    }
}