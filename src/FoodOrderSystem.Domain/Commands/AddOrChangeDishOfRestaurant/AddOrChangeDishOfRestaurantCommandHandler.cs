using FoodOrderSystem.Domain.Model;
using FoodOrderSystem.Domain.Model.Dish;
using FoodOrderSystem.Domain.Model.DishCategory;
using FoodOrderSystem.Domain.Model.Restaurant;
using FoodOrderSystem.Domain.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Domain.Commands.AddOrChangeDishOfRestaurant
{
    public class AddOrChangeDishOfRestaurantCommandHandler : ICommandHandler<AddOrChangeDishOfRestaurantCommand, Guid>
    {
        private readonly IRestaurantRepository restaurantRepository;
        private readonly IDishCategoryRepository dishCategoryRepository;
        private readonly IDishRepository dishRepository;

        public AddOrChangeDishOfRestaurantCommandHandler(IRestaurantRepository restaurantRepository, IDishCategoryRepository dishCategoryRepository, IDishRepository dishRepository)
        {
            this.restaurantRepository = restaurantRepository;
            this.dishCategoryRepository = dishCategoryRepository;
            this.dishRepository = dishRepository;
        }

        public async Task<Result<Guid>> HandleAsync(AddOrChangeDishOfRestaurantCommand command, User currentUser, CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (currentUser == null)
                return FailureResult<Guid>.Unauthorized();

            if (currentUser.Role < Role.RestaurantAdmin)
                return FailureResult<Guid>.Forbidden();

            var restaurant = await restaurantRepository.FindByRestaurantIdAsync(command.RestaurantId, cancellationToken);
            if (restaurant == null)
                return FailureResult<Guid>.Create(FailureResultCode.RestaurantDoesNotExist);

            if (currentUser.Role == Role.RestaurantAdmin && !restaurant.HasAdministrator(currentUser.Id))
                return FailureResult<Guid>.Forbidden();

            var dishCategories = await dishCategoryRepository.FindByRestaurantIdAsync(command.RestaurantId, cancellationToken);
            var dishCategory = dishCategories?.FirstOrDefault(en => en.Id == command.DishCategoryId);
            if (dishCategory == null)
                return FailureResult<Guid>.Create(FailureResultCode.DishCategoryDoesNotBelongToRestaurant);

            Dish dish;
            if (command.Dish.Id != Guid.Empty)
            {
                var dishes = await dishRepository.FindByDishCategoryIdAsync(dishCategory.Id, cancellationToken);
                dish = dishes?.FirstOrDefault(en => en.Id.Value == command.Dish.Id);
                if (dish == null)
                    return FailureResult<Guid>.Create(FailureResultCode.DishDoesNotBelongToDishCategory);
            }
            else
            {
                dish = new Dish(
                    new DishId(Guid.NewGuid()),
                    command.RestaurantId,
                    command.DishCategoryId,
                    command.Dish.Name,
                    command.Dish.Description,
                    command.Dish.ProductInfo,
                    new List<DishVariant>() // TODO
                );
            }

            return SuccessResult<Guid>.Create(dishCategory.Id.Value);
        }
    }
}
