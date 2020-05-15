﻿using FoodOrderSystem.Domain.Model;
using FoodOrderSystem.Domain.Model.Restaurant;
using FoodOrderSystem.Domain.Model.User;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Domain.Commands.RemoveCuisineFromRestaurant
{
    public class RemoveCuisineFromRestaurantCommandHandler : ICommandHandler<RemoveCuisineFromRestaurantCommand, bool>
    {
        private readonly IRestaurantRepository restaurantRepository;

        public RemoveCuisineFromRestaurantCommandHandler(IRestaurantRepository restaurantRepository)
        {
            this.restaurantRepository = restaurantRepository;
        }

        public async Task<Result<bool>> HandleAsync(RemoveCuisineFromRestaurantCommand command, User currentUser, CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (currentUser == null)
                return FailureResult<bool>.Unauthorized();

            if (currentUser.Role < Role.RestaurantAdmin)
                return FailureResult<bool>.Forbidden();

            var restaurant = await restaurantRepository.FindByRestaurantIdAsync(command.RestaurantId, cancellationToken);
            if (restaurant == null)
                return FailureResult<bool>.Create(FailureResultCode.RestaurantDoesNotExist);

            if (currentUser.Role == Role.RestaurantAdmin && !restaurant.HasAdministrator(currentUser.Id))
                return FailureResult<bool>.Forbidden();

            restaurant.RemoveCuisine(command.CuisineId);

            await restaurantRepository.StoreAsync(restaurant, cancellationToken);

            return SuccessResult<bool>.Create(true);
        }
    }
}