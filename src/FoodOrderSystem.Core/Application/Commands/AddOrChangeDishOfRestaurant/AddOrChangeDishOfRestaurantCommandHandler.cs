﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FoodOrderSystem.Core.Application.Ports.Persistence;
using FoodOrderSystem.Core.Common;
using FoodOrderSystem.Core.Domain.Model.Dish;
using FoodOrderSystem.Core.Domain.Model.User;

namespace FoodOrderSystem.Core.Application.Commands.AddOrChangeDishOfRestaurant
{
    public class AddOrChangeDishOfRestaurantCommandHandler : ICommandHandler<AddOrChangeDishOfRestaurantCommand, Guid>
    {
        private readonly IRestaurantRepository restaurantRepository;
        private readonly IDishCategoryRepository dishCategoryRepository;
        private readonly IDishRepository dishRepository;
        private readonly IDishFactory dishFactory;

        public AddOrChangeDishOfRestaurantCommandHandler(IRestaurantRepository restaurantRepository,
            IDishCategoryRepository dishCategoryRepository, IDishRepository dishRepository, IDishFactory dishFactory)
        {
            this.restaurantRepository = restaurantRepository;
            this.dishCategoryRepository = dishCategoryRepository;
            this.dishRepository = dishRepository;
            this.dishFactory = dishFactory;
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
                return FailureResult<Guid>.Create(FailureResultCode.RestaurantDoesNotExist);

            if (currentUser.Role == Role.RestaurantAdmin && !restaurant.HasAdministrator(currentUser.Id))
                return FailureResult<Guid>.Forbidden();

            var dishCategories =
                await dishCategoryRepository.FindByRestaurantIdAsync(command.RestaurantId, cancellationToken);
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

                Result<bool> tempResult;

                if (!string.Equals(dish.Name, command.Dish.Name))
                {
                    tempResult = dish.ChangeName(command.Dish.Name, currentUser.Id);
                    if (tempResult.IsFailure)
                        return tempResult.Cast<Guid>();
                }

                if (!string.Equals(dish.Description, command.Dish.Description))
                {
                    tempResult = dish.ChangeDescription(command.Dish.Description, currentUser.Id);
                    if (tempResult.IsFailure)
                        return tempResult.Cast<Guid>();
                }

                if (!string.Equals(dish.ProductInfo, command.Dish.ProductInfo))
                {
                    tempResult = dish.ChangeProductInfo(command.Dish.ProductInfo, currentUser.Id);
                    if (tempResult.IsFailure)
                        return tempResult.Cast<Guid>();
                }

                if (dish.OrderNo != command.Dish.OrderNo)
                {
                    tempResult = dish.ChangeOrderNo(command.Dish.OrderNo, currentUser.Id);
                    if (tempResult.IsFailure)
                        return tempResult.Cast<Guid>();
                }

                tempResult = dish.ReplaceVariants(command.Dish.Variants, currentUser.Id);
                if (tempResult.IsFailure)
                    return tempResult.Cast<Guid>();
            }
            else
            {
                var createResult = dishFactory.Create(
                    command.RestaurantId,
                    command.DishCategoryId,
                    command.Dish.Name,
                    command.Dish.Description,
                    command.Dish.ProductInfo,
                    command.Dish.OrderNo,
                    command.Dish.Variants,
                    currentUser.Id
                );

                if (createResult.IsFailure)
                    return createResult.Cast<Guid>();

                dish = createResult.Value;
            }

            await dishRepository.StoreAsync(dish, cancellationToken);

            return SuccessResult<Guid>.Create(dish.Id.Value);
        }
    }
}