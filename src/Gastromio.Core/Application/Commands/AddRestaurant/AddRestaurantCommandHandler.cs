using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gastromio.Core.Application.DTOs;
using Gastromio.Core.Application.Ports.Persistence;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;
using Gastromio.Core.Domain.Model.PaymentMethods;
using Gastromio.Core.Domain.Model.Restaurants;
using Gastromio.Core.Domain.Model.Users;

namespace Gastromio.Core.Application.Commands.AddRestaurant
{
    public class AddRestaurantCommandHandler : ICommandHandler<AddRestaurantCommand, RestaurantDTO>
    {
        private readonly IRestaurantFactory restaurantFactory;
        private readonly IRestaurantRepository restaurantRepository;
        private readonly IRestaurantImageRepository restaurantImageRepository;
        private readonly ICuisineRepository cuisineRepository;
        private readonly IPaymentMethodRepository paymentMethodRepository;
        private readonly IUserRepository userRepository;

        public AddRestaurantCommandHandler(
            IRestaurantFactory restaurantFactory,
            IRestaurantRepository restaurantRepository,
            IRestaurantImageRepository restaurantImageRepository,
            ICuisineRepository cuisineRepository,
            IPaymentMethodRepository paymentMethodRepository,
            IUserRepository userRepository)
        {
            this.restaurantFactory = restaurantFactory;
            this.restaurantRepository = restaurantRepository;
            this.restaurantImageRepository = restaurantImageRepository;
            this.cuisineRepository = cuisineRepository;
            this.paymentMethodRepository = paymentMethodRepository;
            this.userRepository = userRepository;
        }

        public async Task<RestaurantDTO> HandleAsync(AddRestaurantCommand command, User currentUser, CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (currentUser == null)
                throw DomainException.CreateFrom(new SessionExpiredFailure());

            if (currentUser.Role < Role.SystemAdmin)
                throw DomainException.CreateFrom(new ForbiddenFailure());

            var existingRestaurants =
                await restaurantRepository.FindByRestaurantNameAsync(command.Name, cancellationToken);
            if (existingRestaurants.Any())
                throw DomainException.CreateFrom(new RestaurantAlreadyExistsFailure());

            var cuisines = (await cuisineRepository.FindAllAsync(cancellationToken))
                .ToDictionary(en => en.Id, en => new CuisineDTO(en));

            var paymentMethods = (await paymentMethodRepository.FindAllAsync(cancellationToken))
                .ToDictionary(en => en.Id, en => new PaymentMethodDTO(en));

            var restaurant = restaurantFactory.CreateWithName(command.Name, currentUser.Id);
            await restaurantRepository.StoreAsync(restaurant, cancellationToken);

            var userIds = restaurant.Administrators
                .Union(new []{restaurant.CreatedBy, restaurant.UpdatedBy})
                .Distinct();

            var users = await userRepository.FindByUserIdsAsync(userIds, cancellationToken);

            var userDict = users != null
                ? users.ToDictionary(user => user.Id, user => new UserDTO(user))
                : new Dictionary<UserId, UserDTO>();

            var restaurantImageTypes =
                await restaurantImageRepository.FindTypesByRestaurantIdsAsync(new[] {restaurant.Id}, cancellationToken);

            return new RestaurantDTO(restaurant, cuisines, paymentMethods, userDict, restaurantImageTypes);
        }
    }
}
